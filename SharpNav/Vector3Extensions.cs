﻿#region License
/**
 * Copyright (c) 2013-2014 Robert Rouhani <robert.rouhani@gmail.com> and other contributors (see CONTRIBUTORS file).
 * Licensed under the MIT License - https://raw.github.com/Robmaister/SharpNav/master/LICENSE
 */
#endregion

using System;
using System.Collections.Generic;

#if MONOGAME || XNA
using Microsoft.Xna.Framework;
#elif OPENTK
using OpenTK;
#elif SHARPDX
using SharpDX;
#endif

namespace SharpNav
{
	/// <summary>
	/// A class that provides extension methods to fix discrepancies between Vector3 implementations.
	/// </summary>
	internal static class Vector3Extensions
	{
#if OPENTK

		/// <summary>
		/// Gets the length of a <see cref="Vector3"/>.
		/// </summary>
		/// <param name="v">A vector.</param>
		/// <returns>The length of the vector.</returns>
		internal static float Length(this Vector3 v)
		{
			return v.Length;
		}

		/// <summary>
		/// Gets the squared length of a <see cref="Vector3"/>. This avoids the square root operation
		/// and is suitable for comparisons.
		/// </summary>
		/// <param name="v">A vector.</param>
		/// <returns>The length of the vector.</returns>
		internal static float LengthSquared(this Vector3 v)
		{
			return v.LengthSquared;
		}

#endif

		internal static void ComponentMin(ref Vector3 left, ref Vector3 right, out Vector3 result)
		{
#if OPENTK || STANDALONE
			Vector3.ComponentMin(ref left, ref right, out result);
#else
			Vector3.Min(ref left, ref right, out result);
#endif
		}

		internal static void ComponentMax(ref Vector3 left, ref Vector3 right, out Vector3 result)
		{
#if OPENTK || STANDALONE
			Vector3.ComponentMax(ref left, ref right, out result);
#else
			Vector3.Max(ref left, ref right, out result);
#endif
		}

		internal static float Distance2D(Vector3 a, Vector3 b)
		{
			float result;
			Distance2D(ref a, ref b, out result);
			return result;
		}

		internal static void Distance2D(ref Vector3 a, ref Vector3 b, out float dist)
		{
			float dx = b.X - a.X;
			float dz = b.Z - a.Z;
			dist = (float)Math.Sqrt(dx * dx + dz * dz);
		}

		/// <summary>
		/// Calculate the dot product of two vectors projected onto the XZ plane.
		/// </summary>
		/// <param name="left">A vector.</param>
		/// <param name="right">Another vector</param>
		/// <param name="result">The dot product of the two vectors.</param>
		internal static void Dot2D(ref Vector3 left, ref Vector3 right, out float result)
		{
			result = left.X * right.X + left.Z * right.Z;
		}

		internal static float Cross2D(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float result;
			Cross2D(ref p1, ref p2, ref p3, out result);
			return result;
		}

		internal static void Cross2D(ref Vector3 p1, ref Vector3 p2, ref Vector3 p3, out float result)
		{
			float u1 = p2.X - p1.X;
			float v1 = p2.Z - p1.Z;
			float u2 = p3.X - p1.X;
			float v2 = p3.Z - p1.Z;

			result = u1 * v2 - v1 * u2;
		}

		internal static void PerpDotXZ(ref Vector3 a, ref Vector3 b, out float result)
		{
			result = a.X * b.Z - a.Z * b.X;
		}

		internal class RoughYEqualityComparer : IEqualityComparer<Vector3>
		{
			private const int HashConstX = unchecked((int)0x8da6b343);
			private const int HashConstZ = unchecked((int)0xcb1ab31f);

			private float epsilonY;

			public RoughYEqualityComparer(float epsilonY)
			{
				this.epsilonY = epsilonY;
			}

			public bool Equals(Vector3 left, Vector3 right)
			{
				return left.X == right.X && (Math.Abs(left.Y - right.Y) <= epsilonY) && left.Z == right.Z;
			}

			public int GetHashCode(Vector3 obj)
			{
				return HashConstX * (int)obj.X + HashConstZ * (int)obj.Z;
			}
		}
	}
}
