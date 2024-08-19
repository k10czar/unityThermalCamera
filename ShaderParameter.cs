using UnityEngine;

[System.Serializable]
public class ColorShaderParameter : ShaderParameter<Color>
{
	protected override void ApplyOnInstanceOf( Material mat ) => mat.SetColor(name, value);
	protected override void ApplyStatically() => Shader.SetGlobalColor(name, value);
}

[System.Serializable]
public class TextureShaderParameter : ShaderParameter<Texture2D>
{
	protected override void ApplyOnInstanceOf( Material mat ) => mat.SetTexture(name, value);
	protected override void ApplyStatically() => Shader.SetGlobalTexture(name, value);
}

[System.Serializable]
public class FloatShaderParameter : ShaderParameter<float>
{
	protected override void ApplyOnInstanceOf( Material mat ) => mat.SetFloat(name, value);
	protected override void ApplyStatically() => Shader.SetGlobalFloat(name, value);
}

[System.Serializable]
public class IntShaderParameter : ShaderParameter<int>
{
	protected override void ApplyOnInstanceOf( Material mat ) => mat.SetInt(name, value);
	protected override void ApplyStatically() => Shader.SetGlobalInt(name, value);
}

[System.Serializable]
public abstract class ShaderParameter
{
	public string name;
	public bool isStatic;
	
	public void ApplyOn(Material mat)
	{
		if( isStatic ) ApplyStatically();
		else ApplyOnInstanceOf( mat );
	}
	protected abstract void ApplyOnInstanceOf( Material mat );
	protected abstract void ApplyStatically();
}

[System.Serializable]
public abstract class ShaderParameter<T> : ShaderParameter
{
	public T value;
}
