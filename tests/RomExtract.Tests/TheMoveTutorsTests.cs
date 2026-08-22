using PokeMmo.RomExtract;
using PokeMmo.RomExtract.Scripts;
using Xunit;

namespace PokeMmo.RomExtract.Tests;

/// <summary>
/// The move tutors: found by shape, confirmed by text (311).
/// <para>
/// 310 left <c>0x02C0</c>-<c>0x02CE</c> unread — a contiguous band of flags, not one of which
/// hides any object. They are one per move tutor, and the move each teaches is in no script: the
/// only thing that varies is <c>setvar 0x8005, N</c>, handed straight to a routine.
/// </para>
/// <para>
/// <b>Nothing here is filtered to produce fifteen.</b> The scripts are found by their shape and
/// then GROUPED by the routine they hand the index to; that one group has fifteen members, the
/// indices 0..14 with no holes, and the flags contiguous. All three are outputs.
/// </para>
/// </summary>
public sealed class TheMoveTutorsTests
{
    private const uint Start = Rom.BaseAddress + 0x100;

    private const uint TheRoutineBlock = Rom.BaseAddress + 0x200;

    private const uint TooMuchInIt = Rom.BaseAddress + 0x240;

    private static byte[] Word(int value) => [(byte)value, (byte)(value >> 8)];

    private static byte[] At(uint address) =>
        [(byte)address, (byte)(address >> 8), (byte)(address >> 16), (byte)(address >> 24)];

    private static Rom Image(params (uint At, byte[] Bytes)[] pieces)
    {
        var image = new byte[0x800];

        foreach ((uint at, byte[] bytes) in pieces)
            bytes.CopyTo(image, (int)(at - Rom.BaseAddress));

        return new Rom(image);
    }

    // ------------------------------------------------- what makes a block "just one routine"

    /// <summary>
    /// A block whose whole content is one <c>special</c> — plus the commands that do nothing to
    /// its answer.
    /// </summary>
    /// <remarks>
    /// The allowed neighbours are LISTED and not skipped by a length check, because "a short
    /// block" and "a block that only asks a routine" are different things and only the second is
    /// this shape. A version that took any short block would group scripts that hand a number to
    /// something else entirely.
    /// </remarks>
    [Fact]
    public void ABlockThatOnlyAsksARoutineIsRecognisedByWhatElseIsInIt()
    {
        Rom rom = Image(
            (TheRoutineBlock, [0x25, .. Word(0x18D), 0x27, 0x03]),          // special, wait, return
            (TooMuchInIt, [0x25, .. Word(0x18D), 0x29, .. Word(0x40), 0x03])); // ...and a setflag

        Assert.Equal(0x18D, TheMoveTutors.OnlySpecialIn(rom, TheRoutineBlock));
        Assert.Null(TheMoveTutors.OnlySpecialIn(rom, TooMuchInIt));
    }

    /// <summary>And a block asking TWO routines names neither — it is not one routine's block.</summary>
    [Fact]
    public void AndABlockAskingTwoRoutinesNamesNeither()
    {
        Rom rom = Image((TheRoutineBlock, [0x25, .. Word(0x18D), 0x25, .. Word(0x171), 0x03]));

        Assert.Null(TheMoveTutors.OnlySpecialIn(rom, TheRoutineBlock));
    }

    // ---------------------------------------------------------------- finding the scripts

    /// <summary>
    /// The shape: a number into the slot, handed STRAIGHT to a routine, and a flag set after.
    /// </summary>
    /// <remarks>
    /// All three parts are asserted, and the routine is collected rather than filtered on — the
    /// grouping is what makes fifteen a finding (79). A script that sets the slot and then does
    /// something else with it is a different shape and is not one of these.
    /// </remarks>
    [Fact]
    public void AScriptIsOneOfTheseWhenItHandsTheSlotStraightToARoutine()
    {
        Rom rom = Image(
            (Start,
            [
                0x16, .. Word(TheMoveTutors.IndexSlot), .. Word(7),   // setvar 0x8005, 7
                0x04, .. At(TheRoutineBlock),                          // call — the routine's block
                0x29, .. Word(0x2CD),                                  // setflag
                0x02,
            ]),
            (TheRoutineBlock, [0x25, .. Word(0x18D), 0x27, 0x03]));

        TheMoveTutors.ATutor found = Assert.Single(
            TheMoveTutors.Find(rom, [new SetsAFlag("14.1", "person 4", Start)]));

        Assert.Equal(7, found.Index);
        Assert.Equal(0x2CD, found.Flag);
        Assert.Equal(0x18D, found.Routine);
    }

    /// <summary>
    /// And a script that puts the number somewhere else, or hands it to nothing, is not one.
    /// </summary>
    /// <remarks>
    /// Two rejections, not one: the slot being wrong and the call being absent are separate
    /// halves of the shape, and a fixture with only one of them cannot tell which half a break
    /// removed.
    /// </remarks>
    [Fact]
    public void AndTwoThingsThatLookLikeItAreNot()
    {
        Rom wrongSlot = Image(
            (Start,
            [
                0x16, .. Word(0x8004), .. Word(7),                    // a different slot
                0x04, .. At(TheRoutineBlock),
                0x29, .. Word(0x2CD),
                0x02,
            ]),
            (TheRoutineBlock, [0x25, .. Word(0x18D), 0x27, 0x03]));

        Assert.Empty(TheMoveTutors.Find(wrongSlot, [new SetsAFlag("14.1", "person 4", Start)]));

        Rom noCall = Image(
            (Start,
            [
                0x16, .. Word(TheMoveTutors.IndexSlot), .. Word(7),
                0x29, .. Word(0x2CD),                                  // straight to the setflag
                0x02,
            ]));

        Assert.Empty(TheMoveTutors.Find(noCall, [new SetsAFlag("14.1", "person 4", Start)]));
    }

    // ------------------------------------------------------------------ hunting the table

    /// <summary>
    /// The table's shape: every entry a move id this cartridge has, and a nought after the last.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The terminator is what separates a table from any stretch of small numbers, and it is the
    /// condition most easily dropped without the count looking wrong — on the real image the
    /// shape alone passes 81 places and the pointer condition takes it to 2.
    /// </para>
    /// <para>
    /// A nought INSIDE the run is refused for the same reason: nought is the terminator, so a
    /// table containing one is empty space read as a table.
    /// </para>
    /// </remarks>
    [Fact]
    public void TheTableIsARunOfMoveIdsEndedByANought()
    {
        Rom good = Image((Rom.BaseAddress + 0x300, [.. Word(5), .. Word(14), .. Word(25), .. Word(0)]));

        Assert.Equal([Rom.BaseAddress + 0x300], TheMoveTutors.Hunt(good, 3, 355));

        // ASSERTED ON THE ADDRESS AND NOT ON THE COUNT. A zero-filled image is full of tables —
        // any three ids followed by the zeros that fill the rest of it pass — so this fixture's
        // own bytes make a second one at 0x302 out of `14, 25, 9`. That is the nop-slide trap in
        // a new shape (fixtures lie #1), and the fix is to name the place rather than count them.
        Rom noTerminator = Image(
            (Rom.BaseAddress + 0x300, [.. Word(5), .. Word(14), .. Word(25), .. Word(9)]));

        Assert.DoesNotContain(Rom.BaseAddress + 0x300, TheMoveTutors.Hunt(noTerminator, 3, 355));

        Rom noughtInside = Image(
            (Rom.BaseAddress + 0x300, [.. Word(5), .. Word(0), .. Word(25), .. Word(0)]));

        Assert.DoesNotContain(Rom.BaseAddress + 0x300, TheMoveTutors.Hunt(noughtInside, 3, 355));

        Rom outOfRange = Image(
            (Rom.BaseAddress + 0x300, [.. Word(5), .. Word(900), .. Word(25), .. Word(0)]));

        Assert.DoesNotContain(Rom.BaseAddress + 0x300, TheMoveTutors.Hunt(outOfRange, 3, 355));
    }

    /// <summary>And how many aligned words in the image hold an address.</summary>
    /// <remarks>
    /// This is the condition that takes the hunt from 81 places to 2, so it does the work of the
    /// whole reading and is worth a test of its own. Aligned, because an unaligned match is a
    /// coincidence in a file this size and not something a routine can load.
    /// </remarks>
    [Fact]
    public void APointerIsCountedOnlyWhereItIsAligned()
    {
        uint target = Rom.BaseAddress + 0x300;

        Assert.Equal(1, TheMoveTutors.PointedAtBy(Image((Rom.BaseAddress + 0x400, At(target))), target));
        Assert.Equal(2, TheMoveTutors.PointedAtBy(
            Image((Rom.BaseAddress + 0x400, At(target)), (Rom.BaseAddress + 0x410, At(target))), target));

        // The same four bytes one off an aligned word: nothing can load it there.
        Assert.Equal(0, TheMoveTutors.PointedAtBy(Image((Rom.BaseAddress + 0x401, At(target))), target));
    }
}
