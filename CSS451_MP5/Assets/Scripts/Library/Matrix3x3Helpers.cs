//
// Downloaded from: https://gist.github.com/anyuser/bf3b6b76937f778636771153d8ba4ff6/revisions
// Date: 11/6/2017
// By: Kelvin Sung


using UnityEngine;
using System.Collections;

public static class Matrix3x3Helpers
{
	public static Matrix3x3 CreateTranslation (Vector2 translation)
	{
		Matrix3x3 m = new Matrix3x3 ();
		m.m00 = 1;
		m.m11 = 1;
		m.m22 = 1;
		m.m02 = translation.x;
		m.m12 = translation.y;
		return m;
	}

	public static Matrix3x3 CreateRotation (float rotation)
	{
		float cos = Mathf.Cos (rotation * Mathf.Deg2Rad);
		float sin = Mathf.Sin (rotation * Mathf.Deg2Rad);

		Matrix3x3 m = new Matrix3x3 ();
		m.m00 = cos;
		m.m01 = -sin;
		m.m10 = sin;
		m.m11 = cos;
		m.m22 = 1;
		return m;
	}

	public static Matrix3x3 CreateScale (Vector2 scale)
	{
		Matrix3x3 m = new Matrix3x3 ();
		m.m00 = scale.x;
		m.m11 = scale.y;
		m.m22 = 1;
		return m;
	}

	public static Matrix3x3 CreateTRS (Vector2 translation, float rotation, Vector2 scale)
	{
		float cos = Mathf.Cos (rotation * Mathf.Deg2Rad);
		float sin = Mathf.Sin (rotation * Mathf.Deg2Rad);

		Matrix3x3 m = new Matrix3x3 ();
		m.m00 = scale.x * cos;
		m.m01 = scale.x * -sin;
		m.m02 = translation.x;
		m.m10 = scale.y * sin;
		m.m11 = scale.y * cos;
		m.m12 = translation.y;
		m.m20 = 0;
		m.m21 = 0;
		m.m22 = 1;
		return m;
	}
}