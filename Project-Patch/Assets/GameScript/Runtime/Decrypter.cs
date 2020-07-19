using MotionFramework.Resource;

/// <summary>
/// 资源解密服务类
/// </summary>
public class Decrypter : IDecryptServices
{
	EDecryptMethod IDecryptServices.DecryptType 
	{ 
		get 
		{ 
			return EDecryptMethod.GetDecryptOffset; 
		}
	}

	byte[] IDecryptServices.GetDecryptBinary(AssetBundleInfo bundleInfo)
	{
		throw new System.NotImplementedException();
	}

	ulong IDecryptServices.GetDecryptOffset(AssetBundleInfo bundleInfo)
	{
		return 32;
	}
}