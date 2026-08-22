using PokeMmo.Core.Battle;
using PokeMmo.Core.World;
using PokeMmo.RomExtract.Maps;

namespace PokeMmo.RomExtract.Scripts;

/// <summary>
/// The move tutors: a flag band, an index, and the table it indexes (311).
/// </summary>
/// <remarks>
/// <para>
/// 310 turned up <c>0x02C0</c>-<c>0x02CE</c> as the only flags adopting the nought stopped the run
/// setting, noted that <b>not one of them hides any object</b>, and left what they are unread. A
/// contiguous band of flags that gates nothing is a table, and this project had never named one.
/// </para>
/// <para>
/// One flag per person, one person per map, and every script the same shape: check the flag, ask
/// a yes-or-no, call a block whose whole content is one <c>special</c>, <c>setvar 0x8005, N</c>,
/// call another, set the flag. <b>The move is in none of it</b> — the only thing that varies
/// between tutors is <c>N</c>, so what each teaches is inside compiled code, indexed by that.
/// </para>
/// <para>
/// <b>Located by SHAPE and confirmed by TEXT</b>, which are two things that cannot have been
/// tuned to agree. The hunt asks for a run of halfwords, all valid move ids, ending at a nought,
/// with exactly one word in the image pointing at it — no move name is used to find it. The
/// confirmation is that each tutor's own dialogue names the move its index selects.
/// </para>
/// </remarks>
public static class TheMoveTutors
{
    /// <summary>The slot a tutor's script puts its index in, before the call that teaches.</summary>
    /// <remarks>
    /// Read rather than chosen: it is the only operand that differs between the fifteen scripts,
    /// which is what makes it the index. 292 established the run of argument slots; this is one
    /// of them.
    /// </remarks>
    public const int IndexSlot = 0x8005;

    /// <summary>One tutor: where the person is, which flag remembers them, and their index.</summary>
    /// <param name="Flag">The flag set once the tutor has taught.</param>
    /// <param name="Index">What the script puts in <see cref="IndexSlot"/>.</param>
    /// <param name="MapId">The map the person stands on.</param>
    /// <param name="What">Which of the map's lists the script came off.</param>
    /// <param name="Address">The script.</param>
    /// <param name="Routine">
    /// The routine the script hands the index to — the <c>special</c> inside the block it calls
    /// straight after the <c>setvar</c>. <b>Not filtered on and not named here</b>: it is
    /// collected so the reader can GROUP by it, because a group of fifteen scripts all handing
    /// one routine an index is a finding and a filter chosen to produce fifteen is not (79).
    /// </param>
    public sealed record ATutor(int Flag, int Index, string MapId, string What, uint Address, int Routine);

    /// <param name="Tutors">Every one found, in index order.</param>
    /// <param name="Table">Where the table of moves is, or null when the hunt found no single one.</param>
    /// <param name="Candidates">
    /// How many places in the image pass the shape test. <b>The reading is only as good as this
    /// is one</b> — and the number for each prefix length is printed beside it, because a floor
    /// that is only ever quoted at full length cannot show where it settled.
    /// </param>
    /// <param name="InTheReversal">The same hunt on the reversed image.</param>
    /// <param name="Named">
    /// How many tutors' own dialogue contains the name of the move their index selects — the
    /// confirmation, and the half that could have come back nought.
    /// </param>
    public sealed record Reading(
        IReadOnlyList<ATutor> Tutors,
        uint? Table,
        int Candidates,
        int InTheReversal,
        IReadOnlyList<(ATutor Tutor, int MoveId, string MoveName, bool NamedInItsOwnText)> Teaches,
        int Named);

    /// <summary>
    /// Every script that sets a flag and puts a number in <see cref="IndexSlot"/> on the way.
    /// </summary>
    /// <remarks>
    /// Found by what it looks like and not by the band: the flags are an OUTPUT of this, so that
    /// "they are contiguous" stays a finding rather than a filter. A script that sets a flag
    /// without touching the slot, or touches the slot without setting a flag, is not one of these.
    /// </remarks>
    public static IReadOnlyList<ATutor> Find(Rom rom, IEnumerable<SetsAFlag> scripts)
    {
        var found = new List<ATutor>();

        foreach (SetsAFlag script in scripts)
        {
            int? index = null;
            int? flag = null;
            int? routine = null;
            var justSet = false;

            foreach (ScriptCommand command in ScriptReader.ReadAll(rom, script.Address))
            {
                if (command.Arguments.Length >= 4
                    && command.Code == SetVar
                    && command.Word() == IndexSlot)
                {
                    index = command.Word(2);
                    justSet = true;
                    continue;
                }

                // THE CALL STRAIGHT AFTER IT, and what is inside. A script that puts a number in
                // a slot and then does something else with it is not this shape; the tutors hand
                // it to a routine immediately, and the block they call has nothing else in it.
                if (justSet && command.Code == ScriptCommands.Call && rom.IsRomAddress(command.Pointer()))
                {
                    routine ??= OnlySpecialIn(rom, command.Pointer());
                }

                justSet = false;

                if (command.Arguments.Length >= 2 && command.Code == SetFlag && index is not null)
                {
                    flag = command.Word();
                    break;
                }
            }

            if (index is { } n && flag is { } f && routine is { } r)
                found.Add(new ATutor(f, n, script.MapId, script.What, script.Address, r));
        }

        return [.. found.OrderBy(t => t.Index)];
    }

    /// <summary>
    /// The routine a block asks, when asking one routine is ALL the block does.
    /// <para>
    /// Null when the block does anything else. The commands allowed alongside are the ones that
    /// do nothing to the answer — a wait, a lock, turning to face somebody, and the return — and
    /// they are listed rather than skipped by a length check, because "short block" and "block
    /// that only asks a routine" are different things and only the second one is this shape.
    /// </para>
    /// </summary>
    public static int? OnlySpecialIn(Rom rom, uint address)
    {
        int? asked = null;

        foreach (ScriptCommand command in ScriptReader.ReadAll(rom, address))
        {
            if (command.Code == Special)
            {
                if (asked is not null) return null;

                asked = command.Word();
                continue;
            }

            if (command.Code is not (WaitState or Lock or FacePlayer or Return)) return null;
        }

        return asked;
    }

    /// <summary>
    /// Every place in an image that reads as a table of this many move ids, ended by a nought and
    /// named by exactly one aligned word.
    /// </summary>
    /// <param name="entries">How many tutors there are — the table's length is not guessed.</param>
    /// <param name="moves">How many moves this cartridge has, so "a valid id" is read and not assumed.</param>
    public static IReadOnlyList<uint> Hunt(Rom rom, int entries, int moves)
    {
        var found = new List<uint>();

        for (int at = 0; at + (entries + 1) * 2 <= rom.Length; at += 2)
        {
            var ok = true;

            for (int i = 0; i < entries && ok; i++)
            {
                int id = rom.ReadU16(at + i * 2);

                // A move id, and never nought — nought is the terminator and a table that starts
                // with one is a run of empty space rather than a table.
                if (id <= 0 || id >= moves) ok = false;
            }

            // Ended by a nought, which is what separates a table from a stretch of small numbers.
            if (!ok || rom.ReadU16(at + entries * 2) != 0) continue;

            // AND SOMETHING HAS TO NAME IT. A table nothing points at is a coincidence in a
            // sixteen-megabyte file; this is the condition that takes the hunt from thousands to
            // one, and the count without it is printed so the reader can see which did the work
            // (79 — a filter is chosen by the floor, never by the answer).
            found.Add(Rom.BaseAddress + (uint)at);
        }

        return found;
    }

    /// <summary>How many aligned words in the image hold this address.</summary>
    public static int PointedAtBy(Rom rom, uint address)
    {
        var count = 0;

        for (int at = 0; at + 4 <= rom.Length; at += 4)
            if (rom.ReadU32(at) == address) count++;

        return count;
    }

    private const byte SetFlag = 0x29;

    private const byte Special = 0x25;

    private const byte WaitState = 0x27;

    private const byte Lock = 0x6A;

    private const byte FacePlayer = 0x5A;

    private const byte Return = 0x03;

    private const byte SetVar = 0x16;
}
