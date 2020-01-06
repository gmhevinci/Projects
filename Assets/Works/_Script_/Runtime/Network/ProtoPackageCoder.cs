using System;
using System.Collections;
using System.Collections.Generic;
using MotionFramework.Network;

public class ProtoPackageCoder : DefaultPackageCoder
{
	public ProtoPackageCoder()
	{
		PackageSizeFieldType = EPackageSizeFieldType.UShort;
		MessageIDFieldType = EMessageIDFieldType.UShort;
	}

	protected override byte[] EncodeInternal(object msgObj)
	{
		throw new NotImplementedException();
	}

	protected override object DecodeInternal(Type classType, byte[] bodyBytes)
	{
		throw new NotImplementedException();
	}
}