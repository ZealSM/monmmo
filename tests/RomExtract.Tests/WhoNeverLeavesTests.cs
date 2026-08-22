using PokeMmo.Core.World;
using PokeMmo.RomExtract;
using PokeMmo.RomExtract.Scripts;
using Xunit;

namespace PokeMmo.RomExtract.Tests;

/// <summary>
/// People nothing in the cartridge's data can take off the map (314).
/// <para>
/// 306 found flag <c>0x0089</c> hiding one person in MT. EMBER's doorway with eight maps
/// behind them and nothing in sixteen megabytes setting it, and offered three ways to answer:
/// leave the door shut, MODEL an opener, or mark the fact in the world file by a derived rule.
/// This is the third, and the rule is the whole of it — no map, no flag and no person is named
/// anywhere to produce the mark.
/// </para>
/// </summary>
public sealed class WhoNeverLeavesTests
{
    /// <summary>
    /// All five conditions, one fixture per condition, each flipped ON ITS OWN.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Five separate assertions rather than one table, because a single case with several
    /// conditions false cannot say <em>which</em> one a break removed — and four of the five
    /// are exclusions that a narrower rule would drop silently. 211 is the milestone about
    /// exactly that: "no <c>setflag</c> names it" is the tempting rule and it is wrong twice
    /// over, because a pickup and a field move both set a hide flag from compiled code.
    /// </para>
    /// <para>
    /// The base case is asserted first so that every negative below is a change of one thing
    /// from something known to pass. Without it a rule that returned false for everything
    /// would satisfy all four negatives.
    /// </para>
    /// </remarks>
    [Fact]
    public void EachOfTheFiveConditionsRefusesTheMarkOnItsOwn()
    {
        // Behind a flag, and none of the four ways out.
        Assert.True(WhoNeverLeaves.Ever(0x89, false, false, false, false));

        // Nobody hides them, so they were never going anywhere and the mark says nothing.
        Assert.False(WhoNeverLeaves.Ever(0, false, false, false, false));

        // A script sets or clears it. The only one of the four that is a script at all.
        Assert.False(WhoNeverLeaves.Ever(0x89, false, false, true, false));

        // 211's first case: it hands something over, so a routine sets the flag on pickup.
        Assert.False(WhoNeverLeaves.Ever(0x89, true, false, false, false));

        // 211's second: a field move shifts it, and that routine sets the flag too.
        Assert.False(WhoNeverLeaves.Ever(0x89, false, true, false, false));

        // And a new game has them hidden already, so "never leaves" is the wrong sentence.
        Assert.False(WhoNeverLeaves.Ever(0x89, false, false, false, true));
    }

    // ------------------------------------------------------------------ over a whole world

    private static MapObject Person(int localId, int hiddenBy, int shiftedBy = 0, int givesItem = 0) =>
        new(localId, 1, 1, 1, Direction.Down, 0, false, 0, 0, 0, 0, 0)
        {
            HiddenBy = hiddenBy,
            ShiftedBy = shiftedBy,
            GivesItemId = givesItem,
            GivesCount = givesItem == 0 ? 0 : 1,
        };

    private static MapData Map(string id, params MapObject[] people) =>
        new(id, "SOMEWHERE", 4, 4, new byte[16]) { Objects = people };

    /// <summary>
    /// The mark lands on the object, and the flags a new game sets keep it off.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A whole-world pass rather than the predicate alone, because the wiring is where the
    /// mark can go wrong quietly: written onto the wrong object, dropped by a rebuild of the
    /// map record, or computed from the wrong flag list. The image here holds no scripts, so
    /// nothing is moved by one and the two exclusions under test are the object's own and the
    /// starting flags'.
    /// </para>
    /// <para>
    /// The starting flag is passed in rather than read, for the reason 211 gives about levers:
    /// what a new game sets is a fact about the cartridge, and this fixture is about whether
    /// the mark respects it.
    /// </para>
    /// </remarks>
    [Fact]
    public void TheMarkGoesOnThePersonAndANewGamesFlagsKeepItOff()
    {
        var rom = new Rom(new byte[0x1000]);

        var maps = new List<MapData>
        {
            Map("1.97", Person(1, 0x89), Person(2, 0x8A), Person(3, 0)),
        };

        (IReadOnlyList<MapData> marked, TheAlwaysThere reading) =
            WhoNeverLeaves.Mark(rom, maps, [0x8A]);

        IReadOnlyList<MapObject> people = marked[0].Objects;

        Assert.True(people[0].NeverLeaves);       // behind a flag nothing touches
        Assert.False(people[1].NeverLeaves);      // hidden from the first frame
        Assert.False(people[2].NeverLeaves);      // behind no flag at all

        Assert.Equal(1, reading.Marked);
        Assert.Equal(2, reading.BehindAFlag);
        Assert.Equal(3, reading.Objects);
        Assert.Equal([0x89], reading.Flags);
        Assert.Equal(1, reading.OnAtTheStart);
    }

    /// <summary>
    /// A pickup and an obstacle are excluded off the OBJECT, with no script in the image.
    /// </summary>
    /// <remarks>
    /// This is the fixture 211's lesson is for. Both of these are behind a flag that no
    /// <c>setflag</c> anywhere names — the image is empty — and neither is one nothing can
    /// remove, because the routine that hands you the item and the routine that shifts the
    /// rock each set the flag in compiled code. A rule that asked only about scripts would
    /// mark both, and its counts would look entirely reasonable.
    /// </remarks>
    [Fact]
    public void APickupAndAnObstacleAreExcludedWithoutAnyScriptSayingSo()
    {
        var rom = new Rom(new byte[0x1000]);

        var maps = new List<MapData>
        {
            Map("2.0", Person(1, 0x40, givesItem: 4), Person(2, 0x41, shiftedBy: 3), Person(3, 0x42)),
        };

        (IReadOnlyList<MapData> marked, TheAlwaysThere reading) = WhoNeverLeaves.Mark(rom, maps, []);

        Assert.False(marked[0].Objects[0].NeverLeaves);
        Assert.False(marked[0].Objects[1].NeverLeaves);
        Assert.True(marked[0].Objects[2].NeverLeaves);

        Assert.Equal(1, reading.CanBeTakenAway);
        Assert.Equal(1, reading.AnObstacle);
        Assert.Equal([0x42], reading.Flags);
    }

    /// <summary>
    /// A <c>setflag</c> anywhere in the image takes the mark off, wherever it is.
    /// </summary>
    /// <remarks>
    /// The sweep is over every byte and not over the scripts the maps reach, which is the
    /// distinction <see cref="EverywhereInTheImage.EveryFlagMoved"/> was built for: a flag set
    /// by a script nothing leads to is still a flag this cartridge's data can set, and the
    /// person behind it is not one nothing can remove. The flag here is moved from an address
    /// no map in the fixture points at.
    /// </remarks>
    [Fact]
    public void AFlagSetAnywhereInTheImageTakesTheMarkOff()
    {
        var image = new byte[0x1000];

        // setflag 0x0042, end — at an address nothing points at.
        image[0x600] = 0x29;
        image[0x601] = 0x42;
        image[0x602] = 0x00;
        image[0x603] = 0x02;

        var maps = new List<MapData> { Map("2.0", Person(1, 0x42), Person(2, 0x43)) };

        (IReadOnlyList<MapData> marked, TheAlwaysThere reading) =
            WhoNeverLeaves.Mark(new Rom(image), maps, []);

        Assert.False(marked[0].Objects[0].NeverLeaves);
        Assert.True(marked[0].Objects[1].NeverLeaves);
        Assert.Equal(1, reading.MovedByAScript);
    }

    /// <summary>
    /// The mark survives a round trip through the world file.
    /// </summary>
    /// <remarks>
    /// Because that is the whole point of putting it there. 306's third option was to make the
    /// fact something anybody reading the record can have, rather than something this
    /// project's walker works out; a mark that does not travel is the first option wearing the
    /// third one's clothes.
    /// </remarks>
    [Fact]
    public void AndItTravelsInTheWorldFile()
    {
        var world = new WorldData([Map("1.97", Person(1, 0x89) with { NeverLeaves = true }, Person(2, 0x8A))]);

        using var buffer = new MemoryStream();

        world.Save(buffer);
        buffer.Position = 0;

        IReadOnlyList<MapObject> back = WorldData.Load(buffer).Maps.Single().Objects;

        Assert.True(back[0].NeverLeaves);
        Assert.False(back[1].NeverLeaves);
    }
}
