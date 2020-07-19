using System;

/// <summary>
/// 资源打包机密类
/// </summary>
public static class AssetEncrypter
{
	public static bool Check(string filePath)
	{
		// 注意：我们对Entity文件夹内的资源进行了加密
		return filePath.Contains("/entity/");
	}

	public static byte[] Encrypt(byte[] fileData)
	{
		int offset = 32;
		var temper = new byte[fileData.Length + offset];
		Buffer.BlockCopy(fileData, 0, temper, offset, fileData.Length);
		return temper;
	}
}