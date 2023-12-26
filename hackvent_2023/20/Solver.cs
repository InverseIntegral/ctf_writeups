using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;

public struct CandyCaneBlock {
  public uint Expiration;
  public uint Generation;
  public byte Product;
  public byte Flags;
  public ushort Count;
  public ushort Random;
  public byte Type;
  public byte Shuffle;
}

public static class CandyCane {
  public static readonly byte[] candyMap = new byte[256] {
    byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 0,
      (byte) 1,
      (byte) 2,
      (byte) 3,
      (byte) 4,
      (byte) 5,
      (byte) 6,
      (byte) 7,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 8,
      (byte) 9,
      (byte) 10,
      (byte) 11,
      (byte) 12,
      (byte) 13,
      (byte) 14,
      (byte) 15,
      byte.MaxValue,
      (byte) 16,
      (byte) 17,
      (byte) 18,
      (byte) 19,
      (byte) 20,
      byte.MaxValue,
      (byte) 21,
      (byte) 22,
      (byte) 23,
      (byte) 24,
      (byte) 25,
      (byte) 26,
      (byte) 27,
      (byte) 28,
      (byte) 29,
      (byte) 30,
      (byte) 31,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue
  };
  private static readonly byte[] candyMixVertical00 = new byte[24] {
    (byte) 23,
    (byte) 9,
    (byte) 22,
    (byte) 21,
    (byte) 11,
    (byte) 15,
    (byte) 13,
    (byte) 16,
    (byte) 17,
    (byte) 4,
    (byte) 10,
    (byte) 3,
    (byte) 19,
    (byte) 7,
    (byte) 18,
    (byte) 1,
    (byte) 5,
    (byte) 6,
    (byte) 20,
    (byte) 12,
    (byte) 2,
    (byte) 0,
    (byte) 14,
    (byte) 8
  };
  private static readonly byte[] candyMixVertical01 = new byte[24] {
    (byte) 10,
    (byte) 13,
    (byte) 9,
    (byte) 18,
    (byte) 12,
    (byte) 7,
    (byte) 2,
    (byte) 22,
    (byte) 16,
    (byte) 0,
    (byte) 23,
    (byte) 17,
    (byte) 4,
    (byte) 19,
    (byte) 15,
    (byte) 6,
    (byte) 8,
    (byte) 20,
    (byte) 1,
    (byte) 5,
    (byte) 14,
    (byte) 21,
    (byte) 11,
    (byte) 3
  };
  private static readonly byte[] candyMixVertical02 = new byte[24] {
    (byte) 21,
    (byte) 6,
    (byte) 19,
    (byte) 15,
    (byte) 5,
    (byte) 0,
    (byte) 17,
    (byte) 18,
    (byte) 3,
    (byte) 22,
    (byte) 7,
    (byte) 16,
    (byte) 8,
    (byte) 14,
    (byte) 1,
    (byte) 23,
    (byte) 9,
    (byte) 10,
    (byte) 11,
    (byte) 12,
    (byte) 13,
    (byte) 4,
    (byte) 2,
    (byte) 20
  };
  private static readonly byte[] candyMixVertical03 = new byte[24] {
    (byte) 22,
    (byte) 8,
    (byte) 15,
    (byte) 7,
    (byte) 1,
    (byte) 14,
    (byte) 2,
    (byte) 16,
    (byte) 3,
    (byte) 12,
    (byte) 21,
    (byte) 4,
    (byte) 19,
    (byte) 20,
    (byte) 10,
    (byte) 5,
    (byte) 18,
    (byte) 11,
    (byte) 17,
    (byte) 0,
    (byte) 6,
    (byte) 9,
    (byte) 23,
    (byte) 13
  };
  private static readonly byte[] candyMixVertical04 = new byte[24] {
    (byte) 18,
    (byte) 19,
    (byte) 1,
    (byte) 2,
    (byte) 6,
    (byte) 20,
    (byte) 5,
    (byte) 14,
    (byte) 23,
    (byte) 22,
    (byte) 21,
    (byte) 17,
    (byte) 8,
    (byte) 4,
    (byte) 10,
    (byte) 11,
    (byte) 3,
    (byte) 9,
    (byte) 0,
    (byte) 7,
    (byte) 16,
    (byte) 12,
    (byte) 13,
    (byte) 15
  };
  private static readonly byte[] candyMixVertical05 = new byte[24] {
    (byte) 22,
    (byte) 15,
    (byte) 23,
    (byte) 12,
    (byte) 7,
    (byte) 1,
    (byte) 11,
    (byte) 2,
    (byte) 17,
    (byte) 10,
    (byte) 3,
    (byte) 16,
    (byte) 14,
    (byte) 0,
    (byte) 21,
    (byte) 8,
    (byte) 13,
    (byte) 5,
    (byte) 6,
    (byte) 9,
    (byte) 19,
    (byte) 4,
    (byte) 18,
    (byte) 20
  };
  private static readonly byte[] candyMixVertical06 = new byte[24] {
    (byte) 11,
    (byte) 18,
    (byte) 21,
    (byte) 8,
    (byte) 20,
    (byte) 23,
    (byte) 17,
    (byte) 3,
    (byte) 2,
    (byte) 22,
    (byte) 7,
    (byte) 10,
    (byte) 0,
    (byte) 4,
    (byte) 1,
    (byte) 19,
    (byte) 13,
    (byte) 9,
    (byte) 12,
    (byte) 5,
    (byte) 16,
    (byte) 6,
    (byte) 15,
    (byte) 14
  };
  private static readonly byte[] candyMixVertical07 = new byte[24] {
    (byte) 7,
    (byte) 2,
    (byte) 6,
    (byte) 15,
    (byte) 12,
    (byte) 11,
    (byte) 10,
    (byte) 21,
    (byte) 8,
    (byte) 18,
    (byte) 19,
    (byte) 23,
    (byte) 17,
    (byte) 20,
    (byte) 0,
    (byte) 9,
    (byte) 4,
    (byte) 13,
    (byte) 1,
    (byte) 22,
    (byte) 5,
    (byte) 14,
    (byte) 16,
    (byte) 3
  };
  private static readonly byte[] candyMixVertical08 = new byte[24] {
    (byte) 16,
    (byte) 4,
    (byte) 20,
    (byte) 15,
    (byte) 1,
    (byte) 8,
    (byte) 0,
    (byte) 2,
    (byte) 17,
    (byte) 5,
    (byte) 3,
    (byte) 12,
    (byte) 10,
    (byte) 18,
    (byte) 7,
    (byte) 21,
    (byte) 23,
    (byte) 6,
    (byte) 9,
    (byte) 13,
    (byte) 22,
    (byte) 19,
    (byte) 14,
    (byte) 11
  };
  private static readonly byte[] candyMixVertical09 = new byte[24] {
    (byte) 12,
    (byte) 4,
    (byte) 22,
    (byte) 2,
    (byte) 10,
    (byte) 14,
    (byte) 6,
    (byte) 20,
    (byte) 3,
    (byte) 16,
    (byte) 1,
    (byte) 9,
    (byte) 18,
    (byte) 0,
    (byte) 15,
    (byte) 5,
    (byte) 11,
    (byte) 13,
    (byte) 19,
    (byte) 17,
    (byte) 23,
    (byte) 7,
    (byte) 8,
    (byte) 21
  };
  private static readonly byte[] candyMixVertical0A = new byte[24] {
    (byte) 0,
    (byte) 16,
    (byte) 6,
    (byte) 13,
    (byte) 7,
    (byte) 15,
    (byte) 17,
    (byte) 23,
    (byte) 21,
    (byte) 22,
    (byte) 4,
    (byte) 19,
    (byte) 1,
    (byte) 9,
    (byte) 11,
    (byte) 20,
    (byte) 8,
    (byte) 3,
    (byte) 12,
    (byte) 2,
    (byte) 14,
    (byte) 5,
    (byte) 10,
    (byte) 18
  };
  private static readonly byte[] candyMixVertical0B = new byte[24] {
    (byte) 10,
    (byte) 21,
    (byte) 6,
    (byte) 16,
    (byte) 8,
    (byte) 4,
    (byte) 5,
    (byte) 0,
    (byte) 3,
    (byte) 9,
    (byte) 7,
    (byte) 2,
    (byte) 13,
    (byte) 12,
    (byte) 11,
    (byte) 20,
    (byte) 1,
    (byte) 18,
    (byte) 17,
    (byte) 19,
    (byte) 22,
    (byte) 14,
    (byte) 23,
    (byte) 15
  };
  private static readonly byte[] candyMixVertical0C = new byte[24] {
    (byte) 19,
    (byte) 6,
    (byte) 17,
    (byte) 13,
    (byte) 8,
    (byte) 1,
    (byte) 4,
    (byte) 21,
    (byte) 2,
    (byte) 11,
    (byte) 7,
    (byte) 9,
    (byte) 5,
    (byte) 16,
    (byte) 14,
    (byte) 10,
    (byte) 0,
    (byte) 12,
    (byte) 20,
    (byte) 23,
    (byte) 3,
    (byte) 22,
    (byte) 15,
    (byte) 18
  };
  private static readonly byte[] candyMixVertical0D = new byte[24] {
    (byte) 22,
    (byte) 1,
    (byte) 8,
    (byte) 4,
    (byte) 11,
    (byte) 2,
    (byte) 18,
    (byte) 13,
    (byte) 10,
    (byte) 7,
    (byte) 14,
    (byte) 0,
    (byte) 19,
    (byte) 23,
    (byte) 20,
    (byte) 9,
    (byte) 16,
    (byte) 15,
    (byte) 17,
    (byte) 3,
    (byte) 21,
    (byte) 6,
    (byte) 5,
    (byte) 12
  };
  private static readonly byte[] candyMixVertical0E = new byte[24] {
    (byte) 20,
    (byte) 18,
    (byte) 3,
    (byte) 19,
    (byte) 4,
    (byte) 6,
    (byte) 0,
    (byte) 15,
    (byte) 13,
    (byte) 17,
    (byte) 16,
    (byte) 22,
    (byte) 9,
    (byte) 23,
    (byte) 14,
    (byte) 2,
    (byte) 12,
    (byte) 1,
    (byte) 10,
    (byte) 8,
    (byte) 7,
    (byte) 11,
    (byte) 21,
    (byte) 5
  };
  private static readonly byte[] candyMixVertical0F = new byte[24] {
    (byte) 15,
    (byte) 13,
    (byte) 9,
    (byte) 12,
    (byte) 1,
    (byte) 16,
    (byte) 3,
    (byte) 0,
    (byte) 23,
    (byte) 21,
    (byte) 17,
    (byte) 6,
    (byte) 19,
    (byte) 8,
    (byte) 22,
    (byte) 11,
    (byte) 14,
    (byte) 5,
    (byte) 20,
    (byte) 2,
    (byte) 18,
    (byte) 10,
    (byte) 4,
    (byte) 7
  };
  private static readonly byte[] candyMixVertical10 = new byte[24] {
    (byte) 13,
    (byte) 18,
    (byte) 4,
    (byte) 14,
    (byte) 9,
    (byte) 19,
    (byte) 2,
    (byte) 5,
    (byte) 16,
    (byte) 17,
    (byte) 10,
    (byte) 3,
    (byte) 7,
    (byte) 15,
    (byte) 21,
    (byte) 20,
    (byte) 8,
    (byte) 22,
    (byte) 11,
    (byte) 23,
    (byte) 1,
    (byte) 6,
    (byte) 0,
    (byte) 12
  };
  private static readonly byte[] candyMixVertical11 = new byte[24] {
    (byte) 21,
    (byte) 14,
    (byte) 10,
    (byte) 11,
    (byte) 13,
    (byte) 0,
    (byte) 3,
    (byte) 23,
    (byte) 17,
    (byte) 7,
    (byte) 15,
    (byte) 5,
    (byte) 12,
    (byte) 19,
    (byte) 22,
    (byte) 6,
    (byte) 9,
    (byte) 1,
    (byte) 2,
    (byte) 8,
    (byte) 18,
    (byte) 16,
    (byte) 20,
    (byte) 4
  };
  private static readonly byte[] candyMixVertical12 = new byte[24] {
    (byte) 17,
    (byte) 22,
    (byte) 0,
    (byte) 20,
    (byte) 8,
    (byte) 12,
    (byte) 15,
    (byte) 13,
    (byte) 10,
    (byte) 2,
    (byte) 9,
    (byte) 14,
    (byte) 11,
    (byte) 4,
    (byte) 5,
    (byte) 18,
    (byte) 19,
    (byte) 16,
    (byte) 23,
    (byte) 1,
    (byte) 21,
    (byte) 6,
    (byte) 7,
    (byte) 3
  };
  private static readonly byte[] candyMixVertical13 = new byte[24] {
    (byte) 5,
    (byte) 15,
    (byte) 17,
    (byte) 2,
    (byte) 13,
    (byte) 1,
    (byte) 11,
    (byte) 23,
    (byte) 10,
    (byte) 22,
    (byte) 4,
    (byte) 20,
    (byte) 8,
    (byte) 6,
    (byte) 16,
    (byte) 18,
    (byte) 9,
    (byte) 0,
    (byte) 14,
    (byte) 12,
    (byte) 7,
    (byte) 3,
    (byte) 21,
    (byte) 19
  };
  private static readonly byte[] candyMixVertical14 = new byte[24] {
    (byte) 1,
    (byte) 6,
    (byte) 22,
    (byte) 14,
    (byte) 3,
    (byte) 21,
    (byte) 4,
    (byte) 17,
    (byte) 2,
    (byte) 0,
    (byte) 9,
    (byte) 13,
    (byte) 10,
    (byte) 11,
    (byte) 23,
    (byte) 16,
    (byte) 15,
    (byte) 7,
    (byte) 19,
    (byte) 18,
    (byte) 8,
    (byte) 12,
    (byte) 5,
    (byte) 20
  };
  private static readonly byte[] candyMixVertical15 = new byte[24] {
    (byte) 21,
    (byte) 7,
    (byte) 6,
    (byte) 17,
    (byte) 9,
    (byte) 11,
    (byte) 14,
    (byte) 16,
    (byte) 2,
    (byte) 10,
    (byte) 5,
    (byte) 8,
    (byte) 22,
    (byte) 19,
    (byte) 15,
    (byte) 23,
    (byte) 4,
    (byte) 20,
    (byte) 18,
    (byte) 12,
    (byte) 1,
    (byte) 13,
    (byte) 0,
    (byte) 3
  };
  private static readonly byte[] candyMixVertical16 = new byte[24] {
    (byte) 11,
    (byte) 18,
    (byte) 9,
    (byte) 12,
    (byte) 17,
    (byte) 13,
    (byte) 10,
    (byte) 22,
    (byte) 0,
    (byte) 1,
    (byte) 20,
    (byte) 16,
    (byte) 7,
    (byte) 19,
    (byte) 15,
    (byte) 3,
    (byte) 5,
    (byte) 8,
    (byte) 14,
    (byte) 21,
    (byte) 2,
    (byte) 23,
    (byte) 6,
    (byte) 4
  };
  private static readonly byte[] candyMixVertical17 = new byte[24] {
    (byte) 15,
    (byte) 12,
    (byte) 5,
    (byte) 22,
    (byte) 23,
    (byte) 4,
    (byte) 8,
    (byte) 18,
    (byte) 16,
    (byte) 11,
    (byte) 0,
    (byte) 14,
    (byte) 7,
    (byte) 6,
    (byte) 20,
    (byte) 17,
    (byte) 2,
    (byte) 19,
    (byte) 21,
    (byte) 10,
    (byte) 1,
    (byte) 9,
    (byte) 13,
    (byte) 3
  };
  private static readonly byte[] candyMixVertical18 = new byte[24] {
    (byte) 16,
    (byte) 22,
    (byte) 2,
    (byte) 14,
    (byte) 11,
    (byte) 8,
    (byte) 7,
    (byte) 1,
    (byte) 17,
    (byte) 4,
    (byte) 13,
    (byte) 23,
    (byte) 12,
    (byte) 5,
    (byte) 21,
    (byte) 10,
    (byte) 9,
    (byte) 15,
    (byte) 0,
    (byte) 6,
    (byte) 3,
    (byte) 19,
    (byte) 20,
    (byte) 18
  };
  private static readonly byte[] candyMixVertical19 = new byte[24] {
    (byte) 20,
    (byte) 14,
    (byte) 5,
    (byte) 10,
    (byte) 12,
    (byte) 1,
    (byte) 8,
    (byte) 7,
    (byte) 13,
    (byte) 2,
    (byte) 6,
    (byte) 16,
    (byte) 22,
    (byte) 23,
    (byte) 4,
    (byte) 11,
    (byte) 9,
    (byte) 3,
    (byte) 15,
    (byte) 17,
    (byte) 19,
    (byte) 18,
    (byte) 0,
    (byte) 21
  };
  private static readonly byte[] candyMixVertical1A = new byte[24] {
    (byte) 5,
    (byte) 14,
    (byte) 0,
    (byte) 23,
    (byte) 18,
    (byte) 16,
    (byte) 11,
    (byte) 1,
    (byte) 20,
    (byte) 2,
    (byte) 8,
    (byte) 10,
    (byte) 15,
    (byte) 6,
    (byte) 22,
    (byte) 19,
    (byte) 9,
    (byte) 4,
    (byte) 21,
    (byte) 17,
    (byte) 3,
    (byte) 7,
    (byte) 13,
    (byte) 12
  };
  private static readonly byte[] candyMixVertical1B = new byte[24] {
    (byte) 13,
    (byte) 7,
    (byte) 17,
    (byte) 18,
    (byte) 14,
    (byte) 0,
    (byte) 22,
    (byte) 21,
    (byte) 10,
    (byte) 12,
    (byte) 3,
    (byte) 5,
    (byte) 8,
    (byte) 23,
    (byte) 6,
    (byte) 20,
    (byte) 15,
    (byte) 4,
    (byte) 9,
    (byte) 19,
    (byte) 1,
    (byte) 16,
    (byte) 2,
    (byte) 11
  };
  private static readonly byte[] candyMixVertical1C = new byte[24] {
    (byte) 6,
    (byte) 11,
    (byte) 4,
    (byte) 2,
    (byte) 20,
    (byte) 7,
    (byte) 22,
    (byte) 13,
    (byte) 3,
    (byte) 18,
    (byte) 14,
    (byte) 15,
    (byte) 5,
    (byte) 10,
    (byte) 17,
    (byte) 16,
    (byte) 21,
    (byte) 1,
    (byte) 0,
    (byte) 12,
    (byte) 19,
    (byte) 8,
    (byte) 9,
    (byte) 23
  };
  private static readonly byte[] candyMixVertical1D = new byte[24] {
    (byte) 7,
    (byte) 4,
    (byte) 10,
    (byte) 2,
    (byte) 8,
    (byte) 23,
    (byte) 19,
    (byte) 12,
    (byte) 6,
    (byte) 5,
    (byte) 9,
    (byte) 13,
    (byte) 0,
    (byte) 22,
    (byte) 11,
    (byte) 16,
    (byte) 21,
    (byte) 1,
    (byte) 14,
    (byte) 3,
    (byte) 17,
    (byte) 20,
    (byte) 15,
    (byte) 18
  };
  private static readonly byte[] candyMixVertical1E = new byte[24] {
    (byte) 14,
    (byte) 22,
    (byte) 21,
    (byte) 16,
    (byte) 10,
    (byte) 4,
    (byte) 17,
    (byte) 15,
    (byte) 13,
    (byte) 12,
    (byte) 9,
    (byte) 0,
    (byte) 20,
    (byte) 11,
    (byte) 5,
    (byte) 7,
    (byte) 2,
    (byte) 19,
    (byte) 3,
    (byte) 18,
    (byte) 23,
    (byte) 8,
    (byte) 1,
    (byte) 6
  };
  private static readonly byte[] candyMixVertical1F = new byte[24] {
    (byte) 18,
    (byte) 21,
    (byte) 4,
    (byte) 13,
    (byte) 17,
    (byte) 15,
    (byte) 1,
    (byte) 11,
    (byte) 10,
    (byte) 6,
    (byte) 20,
    (byte) 9,
    (byte) 7,
    (byte) 5,
    (byte) 19,
    (byte) 0,
    (byte) 2,
    (byte) 3,
    (byte) 12,
    (byte) 23,
    (byte) 14,
    (byte) 8,
    (byte) 22,
    (byte) 16
  };
  private static readonly byte[][] candyMixVerticals = new byte[32][] {
    CandyCane.candyMixVertical00,
      CandyCane.candyMixVertical01,
      CandyCane.candyMixVertical02,
      CandyCane.candyMixVertical03,
      CandyCane.candyMixVertical04,
      CandyCane.candyMixVertical05,
      CandyCane.candyMixVertical06,
      CandyCane.candyMixVertical07,
      CandyCane.candyMixVertical08,
      CandyCane.candyMixVertical09,
      CandyCane.candyMixVertical0A,
      CandyCane.candyMixVertical0B,
      CandyCane.candyMixVertical0C,
      CandyCane.candyMixVertical0D,
      CandyCane.candyMixVertical0E,
      CandyCane.candyMixVertical0F,
      CandyCane.candyMixVertical10,
      CandyCane.candyMixVertical11,
      CandyCane.candyMixVertical12,
      CandyCane.candyMixVertical13,
      CandyCane.candyMixVertical14,
      CandyCane.candyMixVertical15,
      CandyCane.candyMixVertical16,
      CandyCane.candyMixVertical17,
      CandyCane.candyMixVertical18,
      CandyCane.candyMixVertical19,
      CandyCane.candyMixVertical1A,
      CandyCane.candyMixVertical1B,
      CandyCane.candyMixVertical1C,
      CandyCane.candyMixVertical1D,
      CandyCane.candyMixVertical1E,
      CandyCane.candyMixVertical1F
  };
  public static readonly byte[] shuffler = new byte[24] {
    (byte) 26,
    (byte) 1,
    (byte) 5,
    (byte) 20,
    (byte) 15,
    (byte) 2,
    (byte) 21,
    (byte) 25,
    (byte) 27,
    (byte) 3,
    (byte) 13,
    (byte) 31,
    (byte) 20,
    (byte) 27,
    (byte) 27,
    (byte) 11,
    (byte) 18,
    (byte) 27,
    (byte) 26,
    (byte) 11,
    (byte) 0,
    (byte) 23,
    (byte) 3,
    (byte) 26
  };
  private static readonly byte[] candyMixHorizontal00 = new byte[32] {
    (byte) 26,
    (byte) 27,
    (byte) 6,
    (byte) 4,
    (byte) 31,
    (byte) 15,
    (byte) 20,
    (byte) 2,
    (byte) 28,
    (byte) 12,
    (byte) 0,
    (byte) 23,
    (byte) 24,
    (byte) 18,
    (byte) 5,
    (byte) 8,
    (byte) 10,
    (byte) 25,
    (byte) 3,
    (byte) 21,
    (byte) 7,
    (byte) 9,
    (byte) 22,
    (byte) 13,
    (byte) 14,
    (byte) 1,
    (byte) 16,
    (byte) 30,
    (byte) 17,
    (byte) 19,
    (byte) 29,
    (byte) 11
  };
  private static readonly byte[] candyMixHorizontal01 = new byte[32] {
    (byte) 9,
    (byte) 6,
    (byte) 30,
    (byte) 22,
    (byte) 20,
    (byte) 28,
    (byte) 5,
    (byte) 31,
    (byte) 0,
    (byte) 24,
    (byte) 21,
    (byte) 2,
    (byte) 4,
    (byte) 27,
    (byte) 16,
    (byte) 12,
    (byte) 29,
    (byte) 18,
    (byte) 25,
    (byte) 17,
    (byte) 11,
    (byte) 26,
    (byte) 1,
    (byte) 19,
    (byte) 10,
    (byte) 8,
    (byte) 3,
    (byte) 14,
    (byte) 15,
    (byte) 13,
    (byte) 7,
    (byte) 23
  };
  private static readonly byte[] candyMixHorizontal02 = new byte[32] {
    (byte) 6,
    (byte) 8,
    (byte) 19,
    (byte) 7,
    (byte) 16,
    (byte) 23,
    (byte) 20,
    (byte) 12,
    (byte) 28,
    (byte) 21,
    (byte) 1,
    (byte) 5,
    (byte) 14,
    (byte) 3,
    (byte) 13,
    (byte) 29,
    (byte) 9,
    (byte) 11,
    (byte) 10,
    (byte) 31,
    (byte) 27,
    (byte) 26,
    (byte) 4,
    (byte) 30,
    (byte) 18,
    (byte) 17,
    (byte) 15,
    (byte) 24,
    (byte) 22,
    (byte) 25,
    (byte) 0,
    (byte) 2
  };
  private static readonly byte[] candyMixHorizontal03 = new byte[32] {
    (byte) 10,
    (byte) 23,
    (byte) 5,
    (byte) 15,
    (byte) 21,
    (byte) 18,
    (byte) 25,
    (byte) 11,
    (byte) 31,
    (byte) 19,
    (byte) 16,
    (byte) 20,
    (byte) 12,
    (byte) 22,
    (byte) 8,
    (byte) 26,
    (byte) 17,
    (byte) 24,
    (byte) 4,
    (byte) 30,
    (byte) 0,
    (byte) 14,
    (byte) 6,
    (byte) 13,
    (byte) 2,
    (byte) 9,
    (byte) 28,
    (byte) 27,
    (byte) 1,
    (byte) 29,
    (byte) 7,
    (byte) 3
  };
  private static readonly byte[] candyMixHorizontal04 = new byte[32] {
    (byte) 26,
    (byte) 14,
    (byte) 11,
    (byte) 18,
    (byte) 24,
    (byte) 8,
    (byte) 17,
    (byte) 6,
    (byte) 31,
    (byte) 23,
    (byte) 28,
    (byte) 9,
    (byte) 3,
    (byte) 1,
    (byte) 7,
    (byte) 16,
    (byte) 15,
    (byte) 19,
    (byte) 13,
    (byte) 2,
    (byte) 29,
    (byte) 10,
    (byte) 22,
    (byte) 27,
    (byte) 30,
    (byte) 0,
    (byte) 12,
    (byte) 25,
    (byte) 5,
    (byte) 4,
    (byte) 21,
    (byte) 20
  };
  private static readonly byte[] candyMixHorizontal05 = new byte[32] {
    (byte) 30,
    (byte) 16,
    (byte) 3,
    (byte) 0,
    (byte) 6,
    (byte) 24,
    (byte) 18,
    (byte) 14,
    (byte) 22,
    (byte) 26,
    (byte) 29,
    (byte) 27,
    (byte) 8,
    (byte) 10,
    (byte) 1,
    (byte) 31,
    (byte) 25,
    (byte) 13,
    (byte) 12,
    (byte) 7,
    (byte) 15,
    (byte) 23,
    (byte) 5,
    (byte) 20,
    (byte) 17,
    (byte) 19,
    (byte) 11,
    (byte) 21,
    (byte) 2,
    (byte) 4,
    (byte) 28,
    (byte) 9
  };
  private static readonly byte[] candyMixHorizontal06 = new byte[32] {
    (byte) 1,
    (byte) 31,
    (byte) 17,
    (byte) 27,
    (byte) 16,
    (byte) 4,
    (byte) 5,
    (byte) 10,
    (byte) 15,
    (byte) 20,
    (byte) 14,
    (byte) 2,
    (byte) 22,
    (byte) 21,
    (byte) 23,
    (byte) 25,
    (byte) 0,
    (byte) 12,
    (byte) 13,
    (byte) 28,
    (byte) 6,
    (byte) 3,
    (byte) 11,
    (byte) 29,
    (byte) 9,
    (byte) 18,
    (byte) 24,
    (byte) 30,
    (byte) 26,
    (byte) 7,
    (byte) 8,
    (byte) 19
  };
  private static readonly byte[] candyMixHorizontal07 = new byte[32] {
    (byte) 15,
    (byte) 31,
    (byte) 18,
    (byte) 25,
    (byte) 1,
    (byte) 21,
    (byte) 3,
    (byte) 29,
    (byte) 6,
    (byte) 2,
    (byte) 27,
    (byte) 11,
    (byte) 24,
    (byte) 28,
    (byte) 0,
    (byte) 30,
    (byte) 4,
    (byte) 19,
    (byte) 20,
    (byte) 23,
    (byte) 7,
    (byte) 12,
    (byte) 22,
    (byte) 14,
    (byte) 16,
    (byte) 9,
    (byte) 26,
    (byte) 10,
    (byte) 5,
    (byte) 17,
    (byte) 13,
    (byte) 8
  };
  private static readonly byte[] candyMixHorizontal08 = new byte[32] {
    (byte) 19,
    (byte) 2,
    (byte) 17,
    (byte) 9,
    (byte) 31,
    (byte) 11,
    (byte) 4,
    (byte) 30,
    (byte) 29,
    (byte) 13,
    (byte) 0,
    (byte) 25,
    (byte) 15,
    (byte) 23,
    (byte) 26,
    (byte) 1,
    (byte) 21,
    (byte) 20,
    (byte) 6,
    (byte) 22,
    (byte) 27,
    (byte) 16,
    (byte) 7,
    (byte) 24,
    (byte) 10,
    (byte) 18,
    (byte) 28,
    (byte) 14,
    (byte) 8,
    (byte) 5,
    (byte) 3,
    (byte) 12
  };
  private static readonly byte[] candyMixHorizontal09 = new byte[32] {
    (byte) 28,
    (byte) 24,
    (byte) 4,
    (byte) 13,
    (byte) 18,
    (byte) 12,
    (byte) 23,
    (byte) 7,
    (byte) 5,
    (byte) 30,
    (byte) 19,
    (byte) 3,
    (byte) 2,
    (byte) 17,
    (byte) 27,
    (byte) 15,
    (byte) 16,
    (byte) 25,
    (byte) 21,
    (byte) 14,
    (byte) 31,
    (byte) 10,
    (byte) 8,
    (byte) 22,
    (byte) 11,
    (byte) 1,
    (byte) 20,
    (byte) 29,
    (byte) 0,
    (byte) 9,
    (byte) 6,
    (byte) 26
  };
  private static readonly byte[] candyMixHorizontal0A = new byte[32] {
    (byte) 8,
    (byte) 13,
    (byte) 14,
    (byte) 31,
    (byte) 21,
    (byte) 11,
    (byte) 16,
    (byte) 25,
    (byte) 28,
    (byte) 5,
    (byte) 2,
    (byte) 1,
    (byte) 22,
    (byte) 24,
    (byte) 17,
    (byte) 15,
    (byte) 10,
    (byte) 23,
    (byte) 7,
    (byte) 9,
    (byte) 19,
    (byte) 29,
    (byte) 20,
    (byte) 18,
    (byte) 4,
    (byte) 30,
    (byte) 27,
    (byte) 6,
    (byte) 0,
    (byte) 3,
    (byte) 26,
    (byte) 12
  };
  private static readonly byte[] candyMixHorizontal0B = new byte[32] {
    (byte) 24,
    (byte) 27,
    (byte) 29,
    (byte) 31,
    (byte) 21,
    (byte) 30,
    (byte) 18,
    (byte) 12,
    (byte) 13,
    (byte) 0,
    (byte) 9,
    (byte) 26,
    (byte) 2,
    (byte) 6,
    (byte) 19,
    (byte) 23,
    (byte) 16,
    (byte) 11,
    (byte) 28,
    (byte) 5,
    (byte) 1,
    (byte) 14,
    (byte) 7,
    (byte) 15,
    (byte) 10,
    (byte) 4,
    (byte) 25,
    (byte) 20,
    (byte) 3,
    (byte) 22,
    (byte) 17,
    (byte) 8
  };
  private static readonly byte[] candyMixHorizontal0C = new byte[32] {
    (byte) 15,
    (byte) 9,
    (byte) 19,
    (byte) 27,
    (byte) 6,
    (byte) 30,
    (byte) 22,
    (byte) 17,
    (byte) 24,
    (byte) 14,
    (byte) 31,
    (byte) 10,
    (byte) 25,
    (byte) 16,
    (byte) 18,
    (byte) 12,
    (byte) 29,
    (byte) 20,
    (byte) 4,
    (byte) 7,
    (byte) 3,
    (byte) 8,
    (byte) 1,
    (byte) 26,
    (byte) 11,
    (byte) 0,
    (byte) 23,
    (byte) 28,
    (byte) 5,
    (byte) 21,
    (byte) 13,
    (byte) 2
  };
  private static readonly byte[] candyMixHorizontal0D = new byte[32] {
    (byte) 5,
    (byte) 13,
    (byte) 1,
    (byte) 23,
    (byte) 31,
    (byte) 18,
    (byte) 27,
    (byte) 12,
    (byte) 20,
    (byte) 15,
    (byte) 14,
    (byte) 8,
    (byte) 7,
    (byte) 29,
    (byte) 24,
    (byte) 11,
    (byte) 30,
    (byte) 3,
    (byte) 26,
    (byte) 17,
    (byte) 19,
    (byte) 25,
    (byte) 21,
    (byte) 22,
    (byte) 0,
    (byte) 10,
    (byte) 4,
    (byte) 28,
    (byte) 2,
    (byte) 16,
    (byte) 6,
    (byte) 9
  };
  private static readonly byte[] candyMixHorizontal0E = new byte[32] {
    (byte) 29,
    (byte) 27,
    (byte) 15,
    (byte) 12,
    (byte) 30,
    (byte) 0,
    (byte) 4,
    (byte) 9,
    (byte) 14,
    (byte) 7,
    (byte) 22,
    (byte) 19,
    (byte) 5,
    (byte) 31,
    (byte) 8,
    (byte) 18,
    (byte) 6,
    (byte) 11,
    (byte) 23,
    (byte) 24,
    (byte) 2,
    (byte) 17,
    (byte) 3,
    (byte) 26,
    (byte) 21,
    (byte) 16,
    (byte) 1,
    (byte) 10,
    (byte) 20,
    (byte) 25,
    (byte) 13,
    (byte) 28
  };
  private static readonly byte[] candyMixHorizontal0F = new byte[32] {
    (byte) 27,
    (byte) 1,
    (byte) 10,
    (byte) 15,
    (byte) 3,
    (byte) 21,
    (byte) 11,
    (byte) 9,
    (byte) 2,
    (byte) 25,
    (byte) 12,
    (byte) 30,
    (byte) 31,
    (byte) 29,
    (byte) 22,
    (byte) 28,
    (byte) 6,
    (byte) 17,
    (byte) 20,
    (byte) 7,
    (byte) 8,
    (byte) 5,
    (byte) 19,
    (byte) 13,
    (byte) 0,
    (byte) 16,
    (byte) 14,
    (byte) 4,
    (byte) 18,
    (byte) 23,
    (byte) 24,
    (byte) 26
  };
  private static readonly byte[] candyMixHorizontal10 = new byte[32] {
    (byte) 27,
    (byte) 26,
    (byte) 5,
    (byte) 20,
    (byte) 17,
    (byte) 25,
    (byte) 15,
    (byte) 10,
    (byte) 9,
    (byte) 28,
    (byte) 21,
    (byte) 7,
    (byte) 2,
    (byte) 8,
    (byte) 0,
    (byte) 23,
    (byte) 6,
    (byte) 24,
    (byte) 31,
    (byte) 3,
    (byte) 4,
    (byte) 11,
    (byte) 22,
    (byte) 13,
    (byte) 1,
    (byte) 12,
    (byte) 16,
    (byte) 30,
    (byte) 19,
    (byte) 14,
    (byte) 18,
    (byte) 29
  };
  private static readonly byte[] candyMixHorizontal11 = new byte[32] {
    (byte) 6,
    (byte) 14,
    (byte) 27,
    (byte) 13,
    (byte) 29,
    (byte) 22,
    (byte) 11,
    (byte) 19,
    (byte) 18,
    (byte) 4,
    (byte) 21,
    (byte) 16,
    (byte) 30,
    (byte) 17,
    (byte) 8,
    (byte) 26,
    (byte) 0,
    (byte) 25,
    (byte) 12,
    (byte) 7,
    (byte) 28,
    (byte) 3,
    (byte) 10,
    (byte) 20,
    (byte) 9,
    (byte) 24,
    (byte) 2,
    (byte) 23,
    (byte) 5,
    (byte) 15,
    (byte) 1,
    (byte) 31
  };
  private static readonly byte[] candyMixHorizontal12 = new byte[32] {
    (byte) 1,
    (byte) 29,
    (byte) 9,
    (byte) 0,
    (byte) 20,
    (byte) 5,
    (byte) 18,
    (byte) 4,
    (byte) 27,
    (byte) 6,
    (byte) 24,
    (byte) 30,
    (byte) 15,
    (byte) 2,
    (byte) 25,
    (byte) 13,
    (byte) 7,
    (byte) 14,
    (byte) 19,
    (byte) 8,
    (byte) 17,
    (byte) 3,
    (byte) 11,
    (byte) 21,
    (byte) 12,
    (byte) 31,
    (byte) 23,
    (byte) 10,
    (byte) 22,
    (byte) 28,
    (byte) 26,
    (byte) 16
  };
  private static readonly byte[] candyMixHorizontal13 = new byte[32] {
    (byte) 16,
    (byte) 30,
    (byte) 24,
    (byte) 5,
    (byte) 28,
    (byte) 1,
    (byte) 27,
    (byte) 29,
    (byte) 11,
    (byte) 21,
    (byte) 14,
    (byte) 26,
    (byte) 8,
    (byte) 4,
    (byte) 13,
    (byte) 3,
    (byte) 2,
    (byte) 6,
    (byte) 9,
    (byte) 25,
    (byte) 23,
    (byte) 7,
    (byte) 10,
    (byte) 20,
    (byte) 0,
    (byte) 17,
    (byte) 22,
    (byte) 18,
    (byte) 12,
    (byte) 15,
    (byte) 19,
    (byte) 31
  };
  private static readonly byte[] candyMixHorizontal14 = new byte[32] {
    (byte) 0,
    (byte) 28,
    (byte) 15,
    (byte) 30,
    (byte) 31,
    (byte) 3,
    (byte) 24,
    (byte) 16,
    (byte) 23,
    (byte) 17,
    (byte) 1,
    (byte) 11,
    (byte) 4,
    (byte) 2,
    (byte) 7,
    (byte) 13,
    (byte) 19,
    (byte) 12,
    (byte) 25,
    (byte) 27,
    (byte) 20,
    (byte) 10,
    (byte) 18,
    (byte) 8,
    (byte) 14,
    (byte) 6,
    (byte) 21,
    (byte) 29,
    (byte) 26,
    (byte) 22,
    (byte) 5,
    (byte) 9
  };
  private static readonly byte[] candyMixHorizontal15 = new byte[32] {
    (byte) 24,
    (byte) 0,
    (byte) 19,
    (byte) 15,
    (byte) 22,
    (byte) 11,
    (byte) 14,
    (byte) 28,
    (byte) 12,
    (byte) 8,
    (byte) 25,
    (byte) 17,
    (byte) 26,
    (byte) 23,
    (byte) 3,
    (byte) 31,
    (byte) 18,
    (byte) 13,
    (byte) 5,
    (byte) 7,
    (byte) 30,
    (byte) 4,
    (byte) 27,
    (byte) 1,
    (byte) 16,
    (byte) 2,
    (byte) 21,
    (byte) 10,
    (byte) 9,
    (byte) 20,
    (byte) 29,
    (byte) 6
  };
  private static readonly byte[] candyMixHorizontal16 = new byte[32] {
    (byte) 14,
    (byte) 25,
    (byte) 1,
    (byte) 15,
    (byte) 28,
    (byte) 26,
    (byte) 27,
    (byte) 10,
    (byte) 13,
    (byte) 22,
    (byte) 19,
    (byte) 9,
    (byte) 3,
    (byte) 18,
    (byte) 23,
    (byte) 2,
    (byte) 21,
    (byte) 0,
    (byte) 6,
    (byte) 16,
    (byte) 4,
    (byte) 12,
    (byte) 8,
    (byte) 24,
    (byte) 29,
    (byte) 17,
    (byte) 11,
    (byte) 30,
    (byte) 20,
    (byte) 31,
    (byte) 5,
    (byte) 7
  };
  private static readonly byte[] candyMixHorizontal17 = new byte[32] {
    (byte) 16,
    (byte) 12,
    (byte) 31,
    (byte) 17,
    (byte) 13,
    (byte) 28,
    (byte) 9,
    (byte) 4,
    (byte) 1,
    (byte) 10,
    (byte) 27,
    (byte) 30,
    (byte) 5,
    (byte) 26,
    (byte) 21,
    (byte) 6,
    (byte) 15,
    (byte) 7,
    (byte) 24,
    (byte) 11,
    (byte) 8,
    (byte) 14,
    (byte) 29,
    (byte) 22,
    (byte) 19,
    (byte) 20,
    (byte) 0,
    (byte) 3,
    (byte) 2,
    (byte) 25,
    (byte) 18,
    (byte) 23
  };
  private static readonly byte[] candyMixHorizontal18 = new byte[32] {
    (byte) 18,
    (byte) 19,
    (byte) 30,
    (byte) 15,
    (byte) 29,
    (byte) 11,
    (byte) 16,
    (byte) 26,
    (byte) 1,
    (byte) 25,
    (byte) 8,
    (byte) 9,
    (byte) 31,
    (byte) 3,
    (byte) 13,
    (byte) 20,
    (byte) 6,
    (byte) 23,
    (byte) 4,
    (byte) 28,
    (byte) 12,
    (byte) 10,
    (byte) 21,
    (byte) 5,
    (byte) 17,
    (byte) 14,
    (byte) 24,
    (byte) 22,
    (byte) 2,
    (byte) 27,
    (byte) 0,
    (byte) 7
  };
  private static readonly byte[] candyMixHorizontal19 = new byte[32] {
    (byte) 26,
    (byte) 15,
    (byte) 13,
    (byte) 22,
    (byte) 21,
    (byte) 0,
    (byte) 16,
    (byte) 17,
    (byte) 28,
    (byte) 8,
    (byte) 29,
    (byte) 20,
    (byte) 4,
    (byte) 14,
    (byte) 27,
    (byte) 3,
    (byte) 19,
    (byte) 24,
    (byte) 23,
    (byte) 30,
    (byte) 9,
    (byte) 5,
    (byte) 25,
    (byte) 10,
    (byte) 6,
    (byte) 31,
    (byte) 18,
    (byte) 11,
    (byte) 2,
    (byte) 7,
    (byte) 1,
    (byte) 12
  };
  private static readonly byte[] candyMixHorizontal1A = new byte[32] {
    (byte) 10,
    (byte) 4,
    (byte) 11,
    (byte) 25,
    (byte) 1,
    (byte) 12,
    (byte) 14,
    (byte) 21,
    (byte) 16,
    (byte) 26,
    (byte) 31,
    (byte) 27,
    (byte) 20,
    (byte) 5,
    (byte) 24,
    (byte) 17,
    (byte) 19,
    (byte) 0,
    (byte) 28,
    (byte) 15,
    (byte) 7,
    (byte) 8,
    (byte) 29,
    (byte) 23,
    (byte) 3,
    (byte) 2,
    (byte) 22,
    (byte) 30,
    (byte) 9,
    (byte) 18,
    (byte) 13,
    (byte) 6
  };
  private static readonly byte[] candyMixHorizontal1B = new byte[32] {
    (byte) 13,
    (byte) 12,
    (byte) 29,
    (byte) 0,
    (byte) 1,
    (byte) 28,
    (byte) 30,
    (byte) 20,
    (byte) 5,
    (byte) 27,
    (byte) 8,
    (byte) 7,
    (byte) 19,
    (byte) 18,
    (byte) 16,
    (byte) 17,
    (byte) 10,
    (byte) 2,
    (byte) 15,
    (byte) 22,
    (byte) 21,
    (byte) 31,
    (byte) 4,
    (byte) 6,
    (byte) 23,
    (byte) 9,
    (byte) 11,
    (byte) 14,
    (byte) 24,
    (byte) 3,
    (byte) 26,
    (byte) 25
  };
  private static readonly byte[] candyMixHorizontal1C = new byte[32] {
    (byte) 21,
    (byte) 23,
    (byte) 19,
    (byte) 28,
    (byte) 1,
    (byte) 10,
    (byte) 6,
    (byte) 17,
    (byte) 9,
    (byte) 16,
    (byte) 13,
    (byte) 8,
    (byte) 3,
    (byte) 29,
    (byte) 26,
    (byte) 2,
    (byte) 7,
    (byte) 0,
    (byte) 27,
    (byte) 22,
    (byte) 15,
    (byte) 5,
    (byte) 14,
    (byte) 12,
    (byte) 20,
    (byte) 25,
    (byte) 18,
    (byte) 24,
    (byte) 4,
    (byte) 31,
    (byte) 30,
    (byte) 11
  };
  private static readonly byte[] candyMixHorizontal1D = new byte[32] {
    (byte) 26,
    (byte) 15,
    (byte) 18,
    (byte) 21,
    (byte) 0,
    (byte) 22,
    (byte) 6,
    (byte) 11,
    (byte) 24,
    (byte) 29,
    (byte) 14,
    (byte) 2,
    (byte) 31,
    (byte) 23,
    (byte) 1,
    (byte) 30,
    (byte) 25,
    (byte) 3,
    (byte) 5,
    (byte) 12,
    (byte) 13,
    (byte) 17,
    (byte) 19,
    (byte) 28,
    (byte) 4,
    (byte) 7,
    (byte) 16,
    (byte) 9,
    (byte) 8,
    (byte) 27,
    (byte) 10,
    (byte) 20
  };
  private static readonly byte[] candyMixHorizontal1E = new byte[32] {
    (byte) 14,
    (byte) 25,
    (byte) 27,
    (byte) 8,
    (byte) 24,
    (byte) 17,
    (byte) 2,
    (byte) 11,
    (byte) 1,
    (byte) 12,
    (byte) 19,
    (byte) 16,
    (byte) 0,
    (byte) 30,
    (byte) 29,
    (byte) 6,
    (byte) 22,
    (byte) 3,
    (byte) 21,
    (byte) 15,
    (byte) 13,
    (byte) 18,
    (byte) 20,
    (byte) 28,
    (byte) 7,
    (byte) 31,
    (byte) 26,
    (byte) 5,
    (byte) 9,
    (byte) 4,
    (byte) 23,
    (byte) 10
  };
  private static readonly byte[] candyMixHorizontal1F = new byte[32] {
    (byte) 12,
    (byte) 10,
    (byte) 11,
    (byte) 20,
    (byte) 19,
    (byte) 8,
    (byte) 18,
    (byte) 6,
    (byte) 0,
    (byte) 28,
    (byte) 29,
    (byte) 26,
    (byte) 15,
    (byte) 23,
    (byte) 27,
    (byte) 31,
    (byte) 1,
    (byte) 5,
    (byte) 30,
    (byte) 13,
    (byte) 25,
    (byte) 16,
    (byte) 7,
    (byte) 2,
    (byte) 4,
    (byte) 17,
    (byte) 14,
    (byte) 22,
    (byte) 24,
    (byte) 9,
    (byte) 21,
    (byte) 3
  };
  public static readonly byte[][] candyMixHorizontals = new byte[32][] {
    CandyCane.candyMixHorizontal00,
      CandyCane.candyMixHorizontal01,
      CandyCane.candyMixHorizontal02,
      CandyCane.candyMixHorizontal03,
      CandyCane.candyMixHorizontal04,
      CandyCane.candyMixHorizontal05,
      CandyCane.candyMixHorizontal06,
      CandyCane.candyMixHorizontal07,
      CandyCane.candyMixHorizontal08,
      CandyCane.candyMixHorizontal09,
      CandyCane.candyMixHorizontal0A,
      CandyCane.candyMixHorizontal0B,
      CandyCane.candyMixHorizontal0C,
      CandyCane.candyMixHorizontal0D,
      CandyCane.candyMixHorizontal0E,
      CandyCane.candyMixHorizontal0F,
      CandyCane.candyMixHorizontal10,
      CandyCane.candyMixHorizontal11,
      CandyCane.candyMixHorizontal12,
      CandyCane.candyMixHorizontal13,
      CandyCane.candyMixHorizontal14,
      CandyCane.candyMixHorizontal15,
      CandyCane.candyMixHorizontal16,
      CandyCane.candyMixHorizontal17,
      CandyCane.candyMixHorizontal18,
      CandyCane.candyMixHorizontal19,
      CandyCane.candyMixHorizontal1A,
      CandyCane.candyMixHorizontal1B,
      CandyCane.candyMixHorizontal1C,
      CandyCane.candyMixHorizontal1D,
      CandyCane.candyMixHorizontal1E,
      CandyCane.candyMixHorizontal1F
  };

  private static bool ArrayToBinary(IReadOnlyList < byte > arr, out byte[] bin) {
    bin = new byte[16];
    int num1 = 0;
    int num2 = 0;
    IReadOnlyList < byte > byteList1 = arr;
    int index1 = num2;
    int num3 = index1 + 1;
    long num4 = (long) byteList1[index1] << 35;
    IReadOnlyList < byte > byteList2 = arr;
    int index2 = num3;
    int num5 = index2 + 1;
    long num6 = (long) byteList2[index2] << 30;
    long num7 = num4 | num6;
    IReadOnlyList < byte > byteList3 = arr;
    int index3 = num5;
    int num8 = index3 + 1;
    long num9 = (long) byteList3[index3] << 25;
    long num10 = num7 | num9;
    IReadOnlyList < byte > byteList4 = arr;
    int index4 = num8;
    int num11 = index4 + 1;
    long num12 = (long) byteList4[index4] << 20;
    long num13 = num10 | num12;
    IReadOnlyList < byte > byteList5 = arr;
    int index5 = num11;
    int num14 = index5 + 1;
    long num15 = (long) byteList5[index5] << 15;
    long num16 = num13 | num15;
    IReadOnlyList < byte > byteList6 = arr;
    int index6 = num14;
    int num17 = index6 + 1;
    long num18 = (long) byteList6[index6] << 10;
    long num19 = num16 | num18;
    IReadOnlyList < byte > byteList7 = arr;
    int index7 = num17;
    int num20 = index7 + 1;
    long num21 = (long) byteList7[index7] << 5;
    long num22 = num19 | num21;
    IReadOnlyList < byte > byteList8 = arr;
    int index8 = num20;
    int num23 = index8 + 1;
    long num24 = (long) byteList8[index8];
    ulong num25 = (ulong)(num22 | num24);
    byte[] numArray1 = bin;
    int index9 = num1;
    int num26 = index9 + 1;
    int num27 = (int)(byte)(num25 >> 32 & (ulong) byte.MaxValue);
    numArray1[index9] = (byte) num27;
    byte[] numArray2 = bin;
    int index10 = num26;
    int num28 = index10 + 1;
    int num29 = (int)(byte)(num25 >> 24 & (ulong) byte.MaxValue);
    numArray2[index10] = (byte) num29;
    byte[] numArray3 = bin;
    int index11 = num28;
    int num30 = index11 + 1;
    int num31 = (int)(byte)(num25 >> 16 & (ulong) byte.MaxValue);
    numArray3[index11] = (byte) num31;
    byte[] numArray4 = bin;
    int index12 = num30;
    int num32 = index12 + 1;
    int num33 = (int)(byte)(num25 >> 8 & (ulong) byte.MaxValue);
    numArray4[index12] = (byte) num33;
    byte[] numArray5 = bin;
    int index13 = num32;
    int num34 = index13 + 1;
    int num35 = (int)(byte)(num25 & (ulong) byte.MaxValue);
    numArray5[index13] = (byte) num35;
    IReadOnlyList < byte > byteList9 = arr;
    int index14 = num23;
    int num36 = index14 + 1;
    long num37 = (long) byteList9[index14] << 35;
    IReadOnlyList < byte > byteList10 = arr;
    int index15 = num36;
    int num38 = index15 + 1;
    long num39 = (long) byteList10[index15] << 30;
    long num40 = num37 | num39;
    IReadOnlyList < byte > byteList11 = arr;
    int index16 = num38;
    int num41 = index16 + 1;
    long num42 = (long) byteList11[index16] << 25;
    long num43 = num40 | num42;
    IReadOnlyList < byte > byteList12 = arr;
    int index17 = num41;
    int num44 = index17 + 1;
    long num45 = (long) byteList12[index17] << 20;
    long num46 = num43 | num45;
    IReadOnlyList < byte > byteList13 = arr;
    int index18 = num44;
    int num47 = index18 + 1;
    long num48 = (long) byteList13[index18] << 15;
    long num49 = num46 | num48;
    IReadOnlyList < byte > byteList14 = arr;
    int index19 = num47;
    int num50 = index19 + 1;
    long num51 = (long) byteList14[index19] << 10;
    long num52 = num49 | num51;
    IReadOnlyList < byte > byteList15 = arr;
    int index20 = num50;
    int num53 = index20 + 1;
    long num54 = (long) byteList15[index20] << 5;
    long num55 = num52 | num54;
    IReadOnlyList < byte > byteList16 = arr;
    int index21 = num53;
    int num56 = index21 + 1;
    long num57 = (long) byteList16[index21];
    ulong num58 = (ulong)(num55 | num57);
    byte[] numArray6 = bin;
    int index22 = num34;
    int num59 = index22 + 1;
    int num60 = (int)(byte)(num58 >> 32 & (ulong) byte.MaxValue);
    numArray6[index22] = (byte) num60;
    byte[] numArray7 = bin;
    int index23 = num59;
    int num61 = index23 + 1;
    int num62 = (int)(byte)(num58 >> 24 & (ulong) byte.MaxValue);
    numArray7[index23] = (byte) num62;
    byte[] numArray8 = bin;
    int index24 = num61;
    int num63 = index24 + 1;
    int num64 = (int)(byte)(num58 >> 16 & (ulong) byte.MaxValue);
    numArray8[index24] = (byte) num64;
    byte[] numArray9 = bin;
    int index25 = num63;
    int num65 = index25 + 1;
    int num66 = (int)(byte)(num58 >> 8 & (ulong) byte.MaxValue);
    numArray9[index25] = (byte) num66;
    byte[] numArray10 = bin;
    int index26 = num65;
    int num67 = index26 + 1;
    int num68 = (int)(byte)(num58 & (ulong) byte.MaxValue);
    numArray10[index26] = (byte) num68;
    IReadOnlyList < byte > byteList17 = arr;
    int index27 = num56;
    int num69 = index27 + 1;
    long num70 = (long) byteList17[index27] << 35;
    IReadOnlyList < byte > byteList18 = arr;
    int index28 = num69;
    int num71 = index28 + 1;
    long num72 = (long) byteList18[index28] << 30;
    long num73 = num70 | num72;
    IReadOnlyList < byte > byteList19 = arr;
    int index29 = num71;
    int num74 = index29 + 1;
    long num75 = (long) byteList19[index29] << 25;
    long num76 = num73 | num75;
    IReadOnlyList < byte > byteList20 = arr;
    int index30 = num74;
    int num77 = index30 + 1;
    long num78 = (long) byteList20[index30] << 20;
    long num79 = num76 | num78;
    IReadOnlyList < byte > byteList21 = arr;
    int index31 = num77;
    int num80 = index31 + 1;
    long num81 = (long) byteList21[index31] << 15;
    long num82 = num79 | num81;
    IReadOnlyList < byte > byteList22 = arr;
    int index32 = num80;
    int num83 = index32 + 1;
    long num84 = (long) byteList22[index32] << 10;
    long num85 = num82 | num84;
    IReadOnlyList < byte > byteList23 = arr;
    int index33 = num83;
    int num86 = index33 + 1;
    long num87 = (long) byteList23[index33] << 5;
    long num88 = num85 | num87;
    IReadOnlyList < byte > byteList24 = arr;
    int index34 = num86;
    int index35 = index34 + 1;
    long num89 = (long) byteList24[index34];
    ulong num90 = (ulong)(num88 | num89);
    byte[] numArray11 = bin;
    int index36 = num67;
    int num91 = index36 + 1;
    int num92 = (int)(byte)(num90 >> 32 & (ulong) byte.MaxValue);
    numArray11[index36] = (byte) num92;
    byte[] numArray12 = bin;
    int index37 = num91;
    int num93 = index37 + 1;
    int num94 = (int)(byte)(num90 >> 24 & (ulong) byte.MaxValue);
    numArray12[index37] = (byte) num94;
    byte[] numArray13 = bin;
    int index38 = num93;
    int num95 = index38 + 1;
    int num96 = (int)(byte)(num90 >> 16 & (ulong) byte.MaxValue);
    numArray13[index38] = (byte) num96;
    byte[] numArray14 = bin;
    int index39 = num95;
    int num97 = index39 + 1;
    int num98 = (int)(byte)(num90 >> 8 & (ulong) byte.MaxValue);
    numArray14[index39] = (byte) num98;
    byte[] numArray15 = bin;
    int index40 = num97;
    int index41 = index40 + 1;
    int num99 = (int)(byte)(num90 & (ulong) byte.MaxValue);
    numArray15[index40] = (byte) num99;
    bin[index41] = (byte)((uint) arr[index35] << 3);
    return true;
  }

  private static bool BinaryToArray(IReadOnlyList < byte > bin, out byte[] arr) {
    arr = new byte[25];
    int num1 = 0;
    int num2 = 0;
    IReadOnlyList < byte > byteList1 = bin;
    int index1 = num2;
    int num3 = index1 + 1;
    long num4 = (long) byteList1[index1] << 32;
    IReadOnlyList < byte > byteList2 = bin;
    int index2 = num3;
    int num5 = index2 + 1;
    long num6 = (long) byteList2[index2] << 24;
    long num7 = num4 | num6;
    IReadOnlyList < byte > byteList3 = bin;
    int index3 = num5;
    int num8 = index3 + 1;
    long num9 = (long) byteList3[index3] << 16;
    long num10 = num7 | num9;
    IReadOnlyList < byte > byteList4 = bin;
    int index4 = num8;
    int num11 = index4 + 1;
    long num12 = (long) byteList4[index4] << 8;
    long num13 = num10 | num12;
    IReadOnlyList < byte > byteList5 = bin;
    int index5 = num11;
    int num14 = index5 + 1;
    long num15 = (long) byteList5[index5];
    ulong num16 = (ulong)(num13 | num15);
    byte[] numArray1 = arr;
    int index6 = num1;
    int num17 = index6 + 1;
    int num18 = (int)(byte)(num16 >> 35 & 31);
    numArray1[index6] = (byte) num18;
    byte[] numArray2 = arr;
    int index7 = num17;
    int num19 = index7 + 1;
    int num20 = (int)(byte)(num16 >> 30 & 31);
    numArray2[index7] = (byte) num20;
    byte[] numArray3 = arr;
    int index8 = num19;
    int num21 = index8 + 1;
    int num22 = (int)(byte)(num16 >> 25 & 31);
    numArray3[index8] = (byte) num22;
    byte[] numArray4 = arr;
    int index9 = num21;
    int num23 = index9 + 1;
    int num24 = (int)(byte)(num16 >> 20 & 31);
    numArray4[index9] = (byte) num24;
    byte[] numArray5 = arr;
    int index10 = num23;
    int num25 = index10 + 1;
    int num26 = (int)(byte)(num16 >> 15 & 31);
    numArray5[index10] = (byte) num26;
    byte[] numArray6 = arr;
    int index11 = num25;
    int num27 = index11 + 1;
    int num28 = (int)(byte)(num16 >> 10 & 31);
    numArray6[index11] = (byte) num28;
    byte[] numArray7 = arr;
    int index12 = num27;
    int num29 = index12 + 1;
    int num30 = (int)(byte)(num16 >> 5 & 31);
    numArray7[index12] = (byte) num30;
    byte[] numArray8 = arr;
    int index13 = num29;
    int num31 = index13 + 1;
    int num32 = (int)(byte)(num16 & 31);
    numArray8[index13] = (byte) num32;
    IReadOnlyList < byte > byteList6 = bin;
    int index14 = num14;
    int num33 = index14 + 1;
    long num34 = (long) byteList6[index14] << 32;
    IReadOnlyList < byte > byteList7 = bin;
    int index15 = num33;
    int num35 = index15 + 1;
    long num36 = (long) byteList7[index15] << 24;
    long num37 = num34 | num36;
    IReadOnlyList < byte > byteList8 = bin;
    int index16 = num35;
    int num38 = index16 + 1;
    long num39 = (long) byteList8[index16] << 16;
    long num40 = num37 | num39;
    IReadOnlyList < byte > byteList9 = bin;
    int index17 = num38;
    int num41 = index17 + 1;
    long num42 = (long) byteList9[index17] << 8;
    long num43 = num40 | num42;
    IReadOnlyList < byte > byteList10 = bin;
    int index18 = num41;
    int num44 = index18 + 1;
    long num45 = (long) byteList10[index18];
    ulong num46 = (ulong)(num43 | num45);
    byte[] numArray9 = arr;
    int index19 = num31;
    int num47 = index19 + 1;
    int num48 = (int)(byte)(num46 >> 35 & 31);
    numArray9[index19] = (byte) num48;
    byte[] numArray10 = arr;
    int index20 = num47;
    int num49 = index20 + 1;
    int num50 = (int)(byte)(num46 >> 30 & 31);
    numArray10[index20] = (byte) num50;
    byte[] numArray11 = arr;
    int index21 = num49;
    int num51 = index21 + 1;
    int num52 = (int)(byte)(num46 >> 25 & 31);
    numArray11[index21] = (byte) num52;
    byte[] numArray12 = arr;
    int index22 = num51;
    int num53 = index22 + 1;
    int num54 = (int)(byte)(num46 >> 20 & 31);
    numArray12[index22] = (byte) num54;
    byte[] numArray13 = arr;
    int index23 = num53;
    int num55 = index23 + 1;
    int num56 = (int)(byte)(num46 >> 15 & 31);
    numArray13[index23] = (byte) num56;
    byte[] numArray14 = arr;
    int index24 = num55;
    int num57 = index24 + 1;
    int num58 = (int)(byte)(num46 >> 10 & 31);
    numArray14[index24] = (byte) num58;
    byte[] numArray15 = arr;
    int index25 = num57;
    int num59 = index25 + 1;
    int num60 = (int)(byte)(num46 >> 5 & 31);
    numArray15[index25] = (byte) num60;
    byte[] numArray16 = arr;
    int index26 = num59;
    int num61 = index26 + 1;
    int num62 = (int)(byte)(num46 & 31);
    numArray16[index26] = (byte) num62;
    IReadOnlyList < byte > byteList11 = bin;
    int index27 = num44;
    int num63 = index27 + 1;
    long num64 = (long) byteList11[index27] << 32;
    IReadOnlyList < byte > byteList12 = bin;
    int index28 = num63;
    int num65 = index28 + 1;
    long num66 = (long) byteList12[index28] << 24;
    long num67 = num64 | num66;
    IReadOnlyList < byte > byteList13 = bin;
    int index29 = num65;
    int num68 = index29 + 1;
    long num69 = (long) byteList13[index29] << 16;
    long num70 = num67 | num69;
    IReadOnlyList < byte > byteList14 = bin;
    int index30 = num68;
    int num71 = index30 + 1;
    long num72 = (long) byteList14[index30] << 8;
    long num73 = num70 | num72;
    IReadOnlyList < byte > byteList15 = bin;
    int index31 = num71;
    int index32 = index31 + 1;
    long num74 = (long) byteList15[index31];
    ulong num75 = (ulong)(num73 | num74);
    byte[] numArray17 = arr;
    int index33 = num61;
    int num76 = index33 + 1;
    int num77 = (int)(byte)(num75 >> 35 & 31);
    numArray17[index33] = (byte) num77;
    byte[] numArray18 = arr;
    int index34 = num76;
    int num78 = index34 + 1;
    int num79 = (int)(byte)(num75 >> 30 & 31);
    numArray18[index34] = (byte) num79;
    byte[] numArray19 = arr;
    int index35 = num78;
    int num80 = index35 + 1;
    int num81 = (int)(byte)(num75 >> 25 & 31);
    numArray19[index35] = (byte) num81;
    byte[] numArray20 = arr;
    int index36 = num80;
    int num82 = index36 + 1;
    int num83 = (int)(byte)(num75 >> 20 & 31);
    numArray20[index36] = (byte) num83;
    byte[] numArray21 = arr;
    int index37 = num82;
    int num84 = index37 + 1;
    int num85 = (int)(byte)(num75 >> 15 & 31);
    numArray21[index37] = (byte) num85;
    byte[] numArray22 = arr;
    int index38 = num84;
    int num86 = index38 + 1;
    int num87 = (int)(byte)(num75 >> 10 & 31);
    numArray22[index38] = (byte) num87;
    byte[] numArray23 = arr;
    int index39 = num86;
    int num88 = index39 + 1;
    int num89 = (int)(byte)(num75 >> 5 & 31);
    numArray23[index39] = (byte) num89;
    byte[] numArray24 = arr;
    int index40 = num88;
    int index41 = index40 + 1;
    int num90 = (int)(byte)(num75 & 31);
    numArray24[index40] = (byte) num90;
    arr[index41] = (byte)((uint) bin[index32] >> 3);
    return true;
  }

  private static byte ComputeShuffle(IReadOnlyList < byte > arr) {
    uint num = 0;
    for (int index = 0; index < 24; ++index)
      num += (uint) arr[index] + (uint) CandyCane.shuffler[index];
    return (byte)(num % 32);
  }

  public static bool DecodeBlock(string str, out CandyCaneBlock block) {
    block = new CandyCaneBlock();
    byte[] arr;
    byte[] bin;
    if (!CandyCane.StringToArray(str, out arr) || !CandyCane.UnshuffleArray(arr) || !CandyCane.ArrayToBinary((IReadOnlyList < byte > ) arr, out bin))
      return false;
    IntPtr num = Marshal.AllocHGlobal(25);
    Marshal.Copy(bin, 0, num, 16);
    block = (CandyCaneBlock) Marshal.PtrToStructure(num, typeof (CandyCaneBlock));
    Marshal.FreeHGlobal(num);
    block.Shuffle = arr[24];
    return true;
  }

  private static bool IsValidTime(uint time) => time >= 1704224660;

  private static bool StringToArray(string str, out byte[] arr) {
    arr = (byte[]) null;
    if (str == null || str.Length != 29 || str[5] != '-' || str[11] != '-' || str[17] != '-' || str[23] != '-')
      return false;
    int num = 0;
    arr = new byte[25];
    for (int index = 0; index < 29; ++index) {
      if (index <= 11) {
        if (index == 5 || index == 11)
          continue;
      } else if (index == 17 || index == 23)
        continue;
      byte candy = CandyCane.candyMap[(int) str[index] & (int) byte.MaxValue];
      if (candy >= (byte) 32)
        return false;
      arr[num++] = candy;
    }
    return true;
  }

  private static bool UnshuffleArray(byte[] arr) {
    byte shuffle = CandyCane.ComputeShuffle((IReadOnlyList < byte > ) arr);
    byte index1 = arr[24];
    if (shuffle >= (byte) 32 || index1 >= (byte) 32)
      return false;
    byte[] numArray = new byte[25];
    byte[] candyMixHorizontal = CandyCane.candyMixHorizontals[(int) index1];
    for (int index2 = 0; index2 < 24; ++index2)
      numArray[index2] = candyMixHorizontal[(int) arr[index2]];
    byte[] candyMixVertical = CandyCane.candyMixVerticals[(int) shuffle];
    for (int index3 = 0; index3 < 24; ++index3)
      arr[(int) candyMixVertical[index3]] = numArray[index3];
    return true;
  }
}

public class ProductLicense {
  private static bool IsDefined < T > (T value) => Enum.IsDefined(typeof (T), (object) value);

  internal static bool IsProductDefined(int value) {
    return ProductLicense.IsDefined < ProductLicense.ProductNames > ((ProductLicense.ProductNames) value);
  }

  internal static bool IsProductTypeDefined(int value) {
    return ProductLicense.IsDefined < ProductLicense.ProductTypes > ((ProductLicense.ProductTypes) value);
  }

  public enum ProductNames {
    CandyCaneMachine,
    CandyCaneMachine2000,
  }

  public enum ProductTypes {
    Standard,
    Advanced,
    Premium,
  }
}

public class CandyCaneLicense {
  public CandyCaneBlock candyCane;

  public static CandyCaneLicense ? Create(string serial) {
    CandyCaneLicense candyCaneLicense = new CandyCaneLicense();
    return CandyCane.DecodeBlock(serial, out candyCaneLicense.candyCane) && candyCaneLicense.RangesAreValid() ? candyCaneLicense : (CandyCaneLicense) null;
  }

  public DateTime GetExpirationDate() {
    return CandyCaneLicense.UnixToDateTime(this.candyCane.Expiration);
  }

  public DateTime GetRegistrationDate() {
    return CandyCaneLicense.UnixToDateTime(this.candyCane.Generation);
  }

  public bool IsExpired() => this.candyCane.Expiration < 1704224660;

  private bool RangesAreValid() {
    return ProductLicense.IsProductDefined((int) this.candyCane.Product) && ProductLicense.IsProductTypeDefined((int) this.candyCane.Type);
  }

  private static DateTime UnixToDateTime(uint unix) {
    return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double) unix).ToUniversalTime();
  }

  public int Count => (int) this.candyCane.Count;

  public DateTime ExpirationDate => this.GetExpirationDate();

  public ProductLicense.ProductNames ProductName {
    get => (ProductLicense.ProductNames) this.candyCane.Product;
  }

  public ProductLicense.ProductTypes ProductType {
    get => (ProductLicense.ProductTypes) this.candyCane.Type;
  }

  public DateTime RegistrationDate => this.GetRegistrationDate();
}

public class Solver {
  public static void Main() {
    string serial = "CGTYE-GECCE-M5PVN-42LUC-C9CC2";
    string alpha = serial.Replace("-", "");

    var random = new Random();

    while (true) {
      string current = "";
      List < char > possibleChars = alpha.ToList < char > ();

      for (int i = 0; i < 29; i++) {

        if (i == 5 || i == 11 || i == 17 || i == 23) {
          current += "-";
          continue;
        }

        int r = random.Next(possibleChars.Count);
        current += possibleChars[r];
        possibleChars.RemoveAt(r);

      }
      var license = CandyCaneLicense.Create(current);

      if (license != null) {
        if (!license.IsExpired()) {
          Console.WriteLine(current); // CLMEC-5CCCC-GP4YT-2VNEE-G9CU2
          break;
        }
      }
    }
  }
}