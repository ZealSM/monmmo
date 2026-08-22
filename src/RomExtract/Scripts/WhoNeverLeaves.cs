using PokeMmo.Core.World;

namespace PokeMmo.RomExtract.Scripts;

/// <summary>
/// A hide flag nothing sets that some script still reads, and where it is read.
/// </summary>
public sealed record AFlagStillRead(int Flag, IReadOnlyList<uint> Sites);

/// <summary>What the mark came to, over the whole image.</summary>
/// <param name="Objects">Every object the cartridge has.</param>
/// <param name="BehindAFlag">How many of them a flag can hide.</param>
/// <param name="Marked">How many of THOSE nothing in the data can hide.</param>
/// <param name="MovedByAScript">
/// How many are excluded because a script somewhere moves their flag. <b>This is the
/// denominator that makes the mark mean anything</b> (25, 79, 313): a count of people nothing
/// removes cannot be read without the count of people something does.
/// </param>
/// <param name="OnAtTheStart">How many are excluded because a new game hides them already.</param>
/// <param name="CanBeTakenAway">How many are excluded as pickups — 211's compiled-code case.</param>
/// <param name="AnObstacle">How many are excluded as things a field move shifts.</param>
/// <param name="Flags">The flags the marked people are behind, in order.</param>
/// <param name="Asked">
/// Of those flags, the ones something shaped like a script <em>asks about</em>. A flag nothing
/// sets and something reads would be a flag the game's own code owns; a flag nothing sets and
/// nothing reads is dead weight, and the two are indistinguishable from the set side alone.
/// <para>
/// <b>Not a finding on its own</b> — read it against <paramref name="AskedInTheReversal"/>. On
/// this cartridge it is one and the floor is three, which means none of them is read.
/// </para>
/// </param>
/// <param name="AskedInTheReversal">
/// <b>The floor under <paramref name="Asked"/>, and the reason it cannot be read alone.</b> The
/// same flag numbers asked of the image reversed end for end — a file with the same byte
/// frequencies, the same clumping and no scripts in it at all. The forward count has to beat this
/// before "a script still asks about it" is a sentence about the cartridge (25, 205).
/// </param>
/// <param name="Where">One line per marked person: the map, the object and its flag.</param>
public sealed record TheAlwaysThere(
    int Objects,
    int BehindAFlag,
    int Marked,
    int MovedByAScript,
    int OnAtTheStart,
    int CanBeTakenAway,
    int AnObstacle,
    IReadOnlyList<int> Flags,
    IReadOnlyList<AFlagStillRead> Asked,
    int AskedInTheReversal,
    IReadOnlyList<string> Where);

/// <summary>
/// People this cartridge's data can never take off the map (314).
/// </summary>
/// <remarks>
/// <para>
/// <b>306 put a decision to the operator and it went unmade for eight milestones.</b> Flag
/// <c>0x0089</c> hides one person standing in MT. EMBER's doorway, eight maps sit behind them, and
/// <em>nothing in sixteen megabytes moves that flag</em>. Three ways to answer it were offered:
/// leave the door shut, MODEL an opener, or <b>mark the door shut-for-ever in the world file by a
/// rule that is derived rather than a hand-written list</b>. This is the third.
/// </para>
/// <para>
/// <b>The mark is on the PERSON and not on the door</b>, which is what makes it a fact about the
/// file. Whether a door is fenced is a question about a walk — it needs a run, a lever setting and
/// a grid — and a world record must not carry a number that moves with a lever (211). Whether a
/// person can ever be taken off the map does not: a set flag hides somebody, so a hide flag
/// nothing can set is a person who is always there. The shut door follows from that, and follows
/// for anyone who reads the record rather than only for this project's walker.
/// </para>
/// <para>
/// <b>Three ways a flag gets set, and only one of them is a script.</b> The narrow reading would
/// be "no <c>setflag</c> names it", and 211 is the milestone about why that is wrong: picking a
/// thing up sets its hide flag inside compiled code, and a field move shifts a tree or a rock the
/// same way. Both are excluded here, off what the OBJECT is rather than off any script — which is
/// the same evidence <see cref="WhyTheGatesAreShut"/> uses for its boundary bucket.
/// </para>
/// <para>
/// <b>What this claims and what it does not.</b> It says no script in this image and no routine
/// this project has read can remove the person. It does not say the game cannot: compiled code is
/// a wall this project respects, and the honest form of the mark is <em>nothing in the data opens
/// this</em>. A client is entitled to draw such a person as scenery; nobody is entitled to
/// conclude the room behind them is unreachable on real hardware.
/// </para>
/// </remarks>
public static class WhoNeverLeaves
{
    /// <summary>
    /// Whether this object is one nothing in the data can take away.
    /// </summary>
    /// <param name="hiddenBy">The object's hide flag. Nought means nobody hides it and it stays.</param>
    /// <param name="canBeTakenAway">
    /// Whether it hands something over — 211's case, where the routine that gives you the thing
    /// sets the hide flag in compiled code and no script anywhere does.
    /// </param>
    /// <param name="isAnObstacle">
    /// Whether a field move shifts it — a tree, a rock, a boulder. Also opened by a routine and
    /// not by a <c>setflag</c>.
    /// </param>
    /// <param name="movedByAScript">Whether any script in the image sets or clears the flag.</param>
    /// <param name="onAtTheStart">
    /// Whether a new game has it on already, in which case the person is gone before anybody
    /// arrives and "never leaves" is the wrong sentence about them.
    /// </param>
    public static bool Ever(
        int hiddenBy,
        bool canBeTakenAway,
        bool isAnObstacle,
        bool movedByAScript,
        bool onAtTheStart) =>
        hiddenBy != 0
        && !onAtTheStart
        && !movedByAScript
        && !canBeTakenAway
        && !isAnObstacle;

    /// <summary>
    /// Puts the mark on every object the rule holds for, and says what it came to.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The three exclusions are gathered from three different places on purpose, because
    /// they are three different mechanisms and only the first is a script:
    /// </para>
    /// <list type="bullet">
    /// <item><b>Scripts</b> — every <c>setflag</c> and <c>clearflag</c> in the image, from
    /// <see cref="EverywhereInTheImage.EveryFlagMoved"/>, which finds them by shape and not
    /// by asking any map about them.</item>
    /// <item><b>Pickups</b> — the object's own record says it hands something over and can
    /// be hidden, which is 211's case: the routine that gives you the thing sets the flag in
    /// compiled code.</item>
    /// <item><b>Field moves</b> — the object carries a move, or every object behind its flag
    /// runs an obstacle's script, which is <see cref="GatesThatAreObstacles"/>'s reading.</item>
    /// </list>
    /// <para>
    /// A new game's flags are the fourth exclusion and they are not a mechanism at all: they
    /// are the state before anybody walks. Somebody hidden from the first frame is gone, and
    /// "never leaves" is the wrong sentence about them however little moves their flag.
    /// </para>
    /// </remarks>
    public static (IReadOnlyList<MapData> Maps, TheAlwaysThere Reading) Mark(
        Rom rom, IReadOnlyList<MapData> maps, IReadOnlyCollection<int> atStart)
    {
        IReadOnlyDictionary<int, IReadOnlyList<FlagSite>> moved =
            EverywhereInTheImage.EveryFlagMoved(rom);

        var obstacles = GatesThatAreObstacles.In(rom, new WorldData([.. maps]))
            .Select(gate => gate.Flag)
            .ToHashSet();

        IReadOnlyDictionary<int, IReadOnlyList<uint>> asked = EverywhereInTheImage.EveryFlagAsked(rom);

        var marked = new List<MapData>(maps.Count);
        var flags = new List<int>();
        var where = new List<string>();

        // The four exclusions, counted where they are applied. They OVERLAP — a ball on the
        // floor is usually behind a flag a script sets too — so these are four answers to
        // "why is this one not marked" and not four disjoint buckets, and the four do not sum
        // to the difference. Said here rather than left for a reader to assume.
        int byScript = 0, atStartCount = 0, pickups = 0, obstacleCount = 0;

        foreach (MapData map in maps)
        {
            var objects = new List<MapObject>(map.Objects.Count);

            foreach (MapObject person in map.Objects)
            {
                bool isAnObstacle = person.ShiftedBy != 0 || obstacles.Contains(person.HiddenBy);

                bool ever = Ever(
                    person.HiddenBy,
                    person.CanBeTakenAway,
                    isAnObstacle,
                    moved.ContainsKey(person.HiddenBy),
                    atStart.Contains(person.HiddenBy));

                if (person.HiddenBy != 0)
                {
                    if (moved.ContainsKey(person.HiddenBy)) byScript++;
                    if (atStart.Contains(person.HiddenBy)) atStartCount++;
                    if (person.CanBeTakenAway) pickups++;
                    if (isAnObstacle) obstacleCount++;
                }

                if (ever)
                {
                    flags.Add(person.HiddenBy);
                    where.Add(
                        $"0x{person.HiddenBy:X4}  {map.Id} person {person.LocalId}  {map.Name}");
                }

                objects.Add(person with { NeverLeaves = ever });
            }

            marked.Add(map with { Objects = objects });
        }

        // AND THE SAME QUESTION OF A FILE WITH NO SCRIPTS IN IT. Reversed end for end, so the
        // byte frequencies and the clumping survive and the meaning does not (205).
        byte[] backwards = rom.Span.ToArray();

        Array.Reverse(backwards);

        IReadOnlyDictionary<int, IReadOnlyList<uint>> nowhere =
            EverywhereInTheImage.EveryFlagAsked(new Rom(backwards));

        return (
            marked,
            new TheAlwaysThere(
                maps.Sum(m => m.Objects.Count),
                maps.Sum(m => m.Objects.Count(o => o.HiddenBy != 0)),
                where.Count,
                byScript,
                atStartCount,
                pickups,
                obstacleCount,
                [.. flags.Distinct().Order()],
                [
                    .. flags.Distinct().Order()
                        .Where(asked.ContainsKey)
                        .Select(f => new AFlagStillRead(f, asked[f])),
                ],
                flags.Distinct().Count(nowhere.ContainsKey),
                where));
    }
}
