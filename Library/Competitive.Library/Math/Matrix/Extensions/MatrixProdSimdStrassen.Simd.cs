﻿// <auto-generated>
// DO NOT CHANGE THIS FILE.
// </auto-generated>
using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
using static Avx2;
using static SimdMontgomery;
    public readonly partial struct SimdStrassenImpl<T>
    {
        [凾(256|512)]
        void MulSimd(Span<Vector256<uint>> s, Span<Vector256<uint>> t, Span<Vector256<uint>> u)
        {
            var r = this.R;
            var m1 = this.M1;
            var m2 = this.M2;
            for (int i = 0; i < B * B8; i++)
            {
                var cmpS = CompareGreaterThan(s[i].AsInt32(), m1.AsInt32()).AsUInt32();
                var cmpT = CompareGreaterThan(t[i].AsInt32(), m1.AsInt32()).AsUInt32();
                var difS = And(cmpS, m1);
                var difT = And(cmpT, m1);
                s[i] = Subtract(s[i], difS);
                t[i] = Subtract(t[i], difT);
            }

            var m1v = m1.GetElement(0);
            var m2v = m2.GetElement(0);

            var zero = new Vector256<uint>();
            var th1 = Vector256.Create(0, m1v, 0, m1v, 0, m1v, 0, m1v).AsInt64();
            var th2 = Vector256.Create(0, m2v, 0, m2v, 0, m2v, 0, m2v).AsInt64();

            
            for (int i = 0; i < B; i += 8) {
              for (int j = 0; j < B8; j += 1) {
                Vector256<ulong> prod0200 = default;Vector256<ulong> prod1300 = default;
                Vector256<ulong> prod0210 = default;Vector256<ulong> prod1310 = default;
                Vector256<ulong> prod0220 = default;Vector256<ulong> prod1320 = default;
                Vector256<ulong> prod0230 = default;Vector256<ulong> prod1330 = default;
                Vector256<ulong> prod0240 = default;Vector256<ulong> prod1340 = default;
                Vector256<ulong> prod0250 = default;Vector256<ulong> prod1350 = default;
                Vector256<ulong> prod0260 = default;Vector256<ulong> prod1360 = default;
                Vector256<ulong> prod0270 = default;Vector256<ulong> prod1370 = default;
                for (int k = 0; k < B; k += 8) {
                  for (int l = 0; l < 8; l++) {
                    Vector256<uint> T0 = t[j * B + k + l];var T130 = Shuffle(T0, 0xF5);
                    var S00 = Vector256.Create(s[(i + 0) * B8 + k / 8].GetElement(l));
var ST0200 = Multiply(S00, T0);
var ST1300 = Multiply(S00, T130);
prod0200 = Add(prod0200, ST0200);
prod1300 = Add(prod1300, ST1300);
                    var S10 = Vector256.Create(s[(i + 1) * B8 + k / 8].GetElement(l));
var ST0210 = Multiply(S10, T0);
var ST1310 = Multiply(S10, T130);
prod0210 = Add(prod0210, ST0210);
prod1310 = Add(prod1310, ST1310);
                    var S20 = Vector256.Create(s[(i + 2) * B8 + k / 8].GetElement(l));
var ST0220 = Multiply(S20, T0);
var ST1320 = Multiply(S20, T130);
prod0220 = Add(prod0220, ST0220);
prod1320 = Add(prod1320, ST1320);
                    var S30 = Vector256.Create(s[(i + 3) * B8 + k / 8].GetElement(l));
var ST0230 = Multiply(S30, T0);
var ST1330 = Multiply(S30, T130);
prod0230 = Add(prod0230, ST0230);
prod1330 = Add(prod1330, ST1330);
                    var S40 = Vector256.Create(s[(i + 4) * B8 + k / 8].GetElement(l));
var ST0240 = Multiply(S40, T0);
var ST1340 = Multiply(S40, T130);
prod0240 = Add(prod0240, ST0240);
prod1340 = Add(prod1340, ST1340);
                    var S50 = Vector256.Create(s[(i + 5) * B8 + k / 8].GetElement(l));
var ST0250 = Multiply(S50, T0);
var ST1350 = Multiply(S50, T130);
prod0250 = Add(prod0250, ST0250);
prod1350 = Add(prod1350, ST1350);
                    var S60 = Vector256.Create(s[(i + 6) * B8 + k / 8].GetElement(l));
var ST0260 = Multiply(S60, T0);
var ST1360 = Multiply(S60, T130);
prod0260 = Add(prod0260, ST0260);
prod1360 = Add(prod1360, ST1360);
                    var S70 = Vector256.Create(s[(i + 7) * B8 + k / 8].GetElement(l));
var ST0270 = Multiply(S70, T0);
var ST1370 = Multiply(S70, T130);
prod0270 = Add(prod0270, ST0270);
prod1370 = Add(prod1370, ST1370);
                  }
                  var cmp0200 = CompareGreaterThan(zero.AsInt64(), prod0200.AsInt64());
var cmp1300 = CompareGreaterThan(zero.AsInt64(), prod1300.AsInt64());
var dif0200 = And(cmp0200, th2);
var dif1300 = And(cmp1300, th2);
prod0200 = Subtract(prod0200, dif0200.AsUInt64());
prod1300 = Subtract(prod1300, dif1300.AsUInt64());
                  var cmp0210 = CompareGreaterThan(zero.AsInt64(), prod0210.AsInt64());
var cmp1310 = CompareGreaterThan(zero.AsInt64(), prod1310.AsInt64());
var dif0210 = And(cmp0210, th2);
var dif1310 = And(cmp1310, th2);
prod0210 = Subtract(prod0210, dif0210.AsUInt64());
prod1310 = Subtract(prod1310, dif1310.AsUInt64());
                  var cmp0220 = CompareGreaterThan(zero.AsInt64(), prod0220.AsInt64());
var cmp1320 = CompareGreaterThan(zero.AsInt64(), prod1320.AsInt64());
var dif0220 = And(cmp0220, th2);
var dif1320 = And(cmp1320, th2);
prod0220 = Subtract(prod0220, dif0220.AsUInt64());
prod1320 = Subtract(prod1320, dif1320.AsUInt64());
                  var cmp0230 = CompareGreaterThan(zero.AsInt64(), prod0230.AsInt64());
var cmp1330 = CompareGreaterThan(zero.AsInt64(), prod1330.AsInt64());
var dif0230 = And(cmp0230, th2);
var dif1330 = And(cmp1330, th2);
prod0230 = Subtract(prod0230, dif0230.AsUInt64());
prod1330 = Subtract(prod1330, dif1330.AsUInt64());
                  var cmp0240 = CompareGreaterThan(zero.AsInt64(), prod0240.AsInt64());
var cmp1340 = CompareGreaterThan(zero.AsInt64(), prod1340.AsInt64());
var dif0240 = And(cmp0240, th2);
var dif1340 = And(cmp1340, th2);
prod0240 = Subtract(prod0240, dif0240.AsUInt64());
prod1340 = Subtract(prod1340, dif1340.AsUInt64());
                  var cmp0250 = CompareGreaterThan(zero.AsInt64(), prod0250.AsInt64());
var cmp1350 = CompareGreaterThan(zero.AsInt64(), prod1350.AsInt64());
var dif0250 = And(cmp0250, th2);
var dif1350 = And(cmp1350, th2);
prod0250 = Subtract(prod0250, dif0250.AsUInt64());
prod1350 = Subtract(prod1350, dif1350.AsUInt64());
                  var cmp0260 = CompareGreaterThan(zero.AsInt64(), prod0260.AsInt64());
var cmp1360 = CompareGreaterThan(zero.AsInt64(), prod1360.AsInt64());
var dif0260 = And(cmp0260, th2);
var dif1360 = And(cmp1360, th2);
prod0260 = Subtract(prod0260, dif0260.AsUInt64());
prod1360 = Subtract(prod1360, dif1360.AsUInt64());
                  var cmp0270 = CompareGreaterThan(zero.AsInt64(), prod0270.AsInt64());
var cmp1370 = CompareGreaterThan(zero.AsInt64(), prod1370.AsInt64());
var dif0270 = And(cmp0270, th2);
var dif1370 = And(cmp1370, th2);
prod0270 = Subtract(prod0270, dif0270.AsUInt64());
prod1370 = Subtract(prod1370, dif1370.AsUInt64());
                }
                
  for (int _ = 0; _ < 2; _++) {
    var cmp02 = CompareGreaterThan(prod0200.AsInt64(), th1);
    var cmp13 = CompareGreaterThan(prod1300.AsInt64(), th1);
    var dif02 = And(cmp02, th1);
    var dif13 = And(cmp13, th1);
    prod0200 = Subtract(prod0200, dif02.AsUInt64());
    prod1300 = Subtract(prod1300, dif13.AsUInt64());
  }
  u[(i + 0) * B8 + j + 0] = Reduce(prod0200.AsUInt32(), prod1300.AsUInt32(), r, m1);
                
  for (int _ = 0; _ < 2; _++) {
    var cmp02 = CompareGreaterThan(prod0210.AsInt64(), th1);
    var cmp13 = CompareGreaterThan(prod1310.AsInt64(), th1);
    var dif02 = And(cmp02, th1);
    var dif13 = And(cmp13, th1);
    prod0210 = Subtract(prod0210, dif02.AsUInt64());
    prod1310 = Subtract(prod1310, dif13.AsUInt64());
  }
  u[(i + 1) * B8 + j + 0] = Reduce(prod0210.AsUInt32(), prod1310.AsUInt32(), r, m1);
                
  for (int _ = 0; _ < 2; _++) {
    var cmp02 = CompareGreaterThan(prod0220.AsInt64(), th1);
    var cmp13 = CompareGreaterThan(prod1320.AsInt64(), th1);
    var dif02 = And(cmp02, th1);
    var dif13 = And(cmp13, th1);
    prod0220 = Subtract(prod0220, dif02.AsUInt64());
    prod1320 = Subtract(prod1320, dif13.AsUInt64());
  }
  u[(i + 2) * B8 + j + 0] = Reduce(prod0220.AsUInt32(), prod1320.AsUInt32(), r, m1);
                
  for (int _ = 0; _ < 2; _++) {
    var cmp02 = CompareGreaterThan(prod0230.AsInt64(), th1);
    var cmp13 = CompareGreaterThan(prod1330.AsInt64(), th1);
    var dif02 = And(cmp02, th1);
    var dif13 = And(cmp13, th1);
    prod0230 = Subtract(prod0230, dif02.AsUInt64());
    prod1330 = Subtract(prod1330, dif13.AsUInt64());
  }
  u[(i + 3) * B8 + j + 0] = Reduce(prod0230.AsUInt32(), prod1330.AsUInt32(), r, m1);
                
  for (int _ = 0; _ < 2; _++) {
    var cmp02 = CompareGreaterThan(prod0240.AsInt64(), th1);
    var cmp13 = CompareGreaterThan(prod1340.AsInt64(), th1);
    var dif02 = And(cmp02, th1);
    var dif13 = And(cmp13, th1);
    prod0240 = Subtract(prod0240, dif02.AsUInt64());
    prod1340 = Subtract(prod1340, dif13.AsUInt64());
  }
  u[(i + 4) * B8 + j + 0] = Reduce(prod0240.AsUInt32(), prod1340.AsUInt32(), r, m1);
                
  for (int _ = 0; _ < 2; _++) {
    var cmp02 = CompareGreaterThan(prod0250.AsInt64(), th1);
    var cmp13 = CompareGreaterThan(prod1350.AsInt64(), th1);
    var dif02 = And(cmp02, th1);
    var dif13 = And(cmp13, th1);
    prod0250 = Subtract(prod0250, dif02.AsUInt64());
    prod1350 = Subtract(prod1350, dif13.AsUInt64());
  }
  u[(i + 5) * B8 + j + 0] = Reduce(prod0250.AsUInt32(), prod1350.AsUInt32(), r, m1);
                
  for (int _ = 0; _ < 2; _++) {
    var cmp02 = CompareGreaterThan(prod0260.AsInt64(), th1);
    var cmp13 = CompareGreaterThan(prod1360.AsInt64(), th1);
    var dif02 = And(cmp02, th1);
    var dif13 = And(cmp13, th1);
    prod0260 = Subtract(prod0260, dif02.AsUInt64());
    prod1360 = Subtract(prod1360, dif13.AsUInt64());
  }
  u[(i + 6) * B8 + j + 0] = Reduce(prod0260.AsUInt32(), prod1360.AsUInt32(), r, m1);
                
  for (int _ = 0; _ < 2; _++) {
    var cmp02 = CompareGreaterThan(prod0270.AsInt64(), th1);
    var cmp13 = CompareGreaterThan(prod1370.AsInt64(), th1);
    var dif02 = And(cmp02, th1);
    var dif13 = And(cmp13, th1);
    prod0270 = Subtract(prod0270, dif02.AsUInt64());
    prod1370 = Subtract(prod1370, dif13.AsUInt64());
  }
  u[(i + 7) * B8 + j + 0] = Reduce(prod0270.AsUInt32(), prod1370.AsUInt32(), r, m1);
              }
            }
        }
    }
}
