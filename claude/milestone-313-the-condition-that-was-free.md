# 313 — the condition that was free

**3564 tests green.** Base: 312, 3563.

312 sharpened a test and named an old question it was owed: 302 hunted the table `0xA2`'s index
selects from, reported **"NOT FOUND"** — 462 four-aligned bases putting all 98 index values on a
ROM address, against a reversed-image floor of nought — and never asked whether anything **loads**
one.

Asked:

```
    of the 462 base(s), 54 are EQUALLED by an aligned word somewhere in the image
                        17 are LOADED by an instruction
```

**A twenty-seven-fold cut, and it does not settle it.**

---

## Because the condition was free where those bases live

All seventeen are in one place. So the question is what that place is:

```
      base        equalled  loaded  dialogue  how much of its own 3864-byte span is a ROM address AT ALL
      0x083A6000         6       6   0 of 98      85 %
      0x083A6010         8       8   0 of 98      86 %
      0x083A6018         1       1   0 of 98      86 %
      0x083A61E0         1       1   0 of 98      85 %
      …
```

**Every one sits in a span that is 78–86% ROM addresses before the question is asked.** So none of
them passed by being a table — they passed by being *inside* a pointer table, where the condition
"all 98 indices land on a ROM address" costs nothing.

For scale, the same measure elsewhere in the file:

```
  ROM addresses in 0x3A6000..0x3A7000: 882 of 1024 words (86%)
  ROM addresses in 0x3A0000..0x3A1000: 531 of 1024 words (52%)
  ROM addresses in 0x100000..0x101000 (code): 77 of 1024 words (8%)
```

And they are **one region rather than seventeen candidates** — 0x083A6000, +0x10, +0x18, +0x1E0 …
the same structure entered at different offsets. That is 312's runner-up one reading over: a hunt
whose survivors are all windows into one thing has found one thing, not several.

**302's answer stands.** What changes is that "not found" was *for want of a way to choose between
462* and is now *because the condition does not discriminate where these bases live* — which is a
fact about the hunt, and measurable.

## The control it never had

`TwoColumnsOfOneKind.HowDense` is what was missing: the share of a base's own span that is a ROM
address **at all**. It is printed beside every candidate now, permanently, because a count of
targets-that-are-addresses cannot be read without it (25, 79).

The general rule, and it is not about this table: **"every one of these N indices lands on a
pointer" is not a condition in a file with pointer tables in it.** The number that means something
is how far above the neighbourhood it is.

---

## The guard

| break | predicted | killed |
|---|---|---|
| the density reads the whole buffer instead of its span | 1 | 1 |

The fixture names three densities — all, none, half — and then asks the half-buffer for its first
word alone and expects 100%. A version that quietly reads everything returns the same number
whatever it is asked, which is the one way this could look right and measure nothing.

---

## What this leaves

* **The species index's table is still not found**, and now there is a shape for what would find
  it: something that discriminates *inside* a pointer region. The dialogue column is the only
  discriminator anyone has tried and it reads 1 of 98 at its best base and **0 of 98** at all
  seventeen the instruction test keeps — so the two disagree, and 2's rule says ask which follows
  fewer edges before believing either.
* **The 0x083A6000 region is unnamed.** It is 86% ROM addresses over at least four kilobytes and
  nothing in this project knows what it is. Several tables have been located near it (the pic and
  palette runs at 0x0823xxxx, a palette run at 0x083AD690) and this is not one of them.
* **Every other "located by a pointer to it" in this project is still owed 312's test**, and now
  313's control as well. The tutors' table survives both; nothing else has been asked.
