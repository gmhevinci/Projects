//--------------------------------------------------
// 自动生成  请勿修改
// 研发人员实现LANG多语言接口
//--------------------------------------------------
using MotionFramework.IO;
using MotionFramework.Config;
using System.Collections.Generic;

namespace Hotfix
{
	public class CfgAvatarTable : ConfigTable
	{
		public string HeadIcon { protected set; get; }
		public string Model { protected set; get; }
		public float ModelScale { protected set; get; }
		public float BuffScale { protected set; get; }
		public List<string> AttackSounds { protected set; get; }
		public string DeadSound { protected set; get; }
		public string DeadEffect { protected set; get; }
		public float HudPointHeight { protected set; get; }

		public override void ReadByte(ByteBuffer byteBuf)
		{
			Id = byteBuf.ReadInt();
			HeadIcon = byteBuf.ReadUTF();
			Model = byteBuf.ReadUTF();
			ModelScale = byteBuf.ReadFloat();
			BuffScale = byteBuf.ReadFloat();
			AttackSounds = byteBuf.ReadListUTF();
			DeadSound = byteBuf.ReadUTF();
			DeadEffect = byteBuf.ReadUTF();
			HudPointHeight = byteBuf.ReadFloat();
		}
	}

	public partial class CfgAvatar : AssetConfig
	{
		protected override ConfigTable ReadTable(ByteBuffer byteBuffer)
		{
			CfgAvatarTable table = new CfgAvatarTable();
			table.ReadByte(byteBuffer);
			return table;
		}
	}
}