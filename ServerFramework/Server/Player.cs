// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: player.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace PlayerInfo {

  #region Enums
  /// <summary>
  ///动画状态   枚举从0开始
  /// </summary>
  public enum AniState {
    Idle = 0,
    Run = 1,
    Attack = 2,
    Death = 3,
  }

  #endregion

  #region Messages
  /// <summary>
  ///玩家信息   类从1 开始
  /// </summary>
  public sealed class PlayerData : pb::IMessage {
    private static readonly pb::MessageParser<PlayerData> _parser = new pb::MessageParser<PlayerData>(() => new PlayerData());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<PlayerData> Parser { get { return _parser; } }

    /// <summary>Field number for the "path" field.</summary>
    public const int PathFieldNumber = 1;
    private string path_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Path {
      get { return path_; }
      set {
        path_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "username" field.</summary>
    public const int UsernameFieldNumber = 2;
    private string username_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Username {
      get { return username_; }
      set {
        username_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "posx" field.</summary>
    public const int PosxFieldNumber = 3;
    private float posx_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Posx {
      get { return posx_; }
      set {
        posx_ = value;
      }
    }

    /// <summary>Field number for the "posz" field.</summary>
    public const int PoszFieldNumber = 4;
    private float posz_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Posz {
      get { return posz_; }
      set {
        posz_ = value;
      }
    }

    /// <summary>Field number for the "rosy" field.</summary>
    public const int RosyFieldNumber = 5;
    private float rosy_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Rosy {
      get { return rosy_; }
      set {
        rosy_ = value;
      }
    }

    /// <summary>Field number for the "userId" field.</summary>
    public const int UserIdFieldNumber = 6;
    private int userId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "aniState" field.</summary>
    public const int AniStateFieldNumber = 7;
    private global::PlayerInfo.AniState aniState_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::PlayerInfo.AniState AniState {
      get { return aniState_; }
      set {
        aniState_ = value;
      }
    }

    /// <summary>Field number for the "nowHp" field.</summary>
    public const int NowHpFieldNumber = 8;
    private int nowHp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int NowHp {
      get { return nowHp_; }
      set {
        nowHp_ = value;
      }
    }

    /// <summary>Field number for the "allHp" field.</summary>
    public const int AllHpFieldNumber = 9;
    private int allHp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int AllHp {
      get { return allHp_; }
      set {
        allHp_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Path.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Path);
      }
      if (Username.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Username);
      }
      if (Posx != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(Posx);
      }
      if (Posz != 0F) {
        output.WriteRawTag(37);
        output.WriteFloat(Posz);
      }
      if (Rosy != 0F) {
        output.WriteRawTag(45);
        output.WriteFloat(Rosy);
      }
      if (UserId != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(UserId);
      }
      if (AniState != 0) {
        output.WriteRawTag(56);
        output.WriteEnum((int) AniState);
      }
      if (NowHp != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(NowHp);
      }
      if (AllHp != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(AllHp);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Path.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Path);
      }
      if (Username.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Username);
      }
      if (Posx != 0F) {
        size += 1 + 4;
      }
      if (Posz != 0F) {
        size += 1 + 4;
      }
      if (Rosy != 0F) {
        size += 1 + 4;
      }
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(UserId);
      }
      if (AniState != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) AniState);
      }
      if (NowHp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(NowHp);
      }
      if (AllHp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(AllHp);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            Path = input.ReadString();
            break;
          }
          case 18: {
            Username = input.ReadString();
            break;
          }
          case 29: {
            Posx = input.ReadFloat();
            break;
          }
          case 37: {
            Posz = input.ReadFloat();
            break;
          }
          case 45: {
            Rosy = input.ReadFloat();
            break;
          }
          case 48: {
            UserId = input.ReadInt32();
            break;
          }
          case 56: {
            aniState_ = (global::PlayerInfo.AniState) input.ReadEnum();
            break;
          }
          case 64: {
            NowHp = input.ReadInt32();
            break;
          }
          case 72: {
            AllHp = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///上线人数
  /// </summary>
  public sealed class OnlinePlayer : pb::IMessage {
    private static readonly pb::MessageParser<OnlinePlayer> _parser = new pb::MessageParser<OnlinePlayer>(() => new OnlinePlayer());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<OnlinePlayer> Parser { get { return _parser; } }

    /// <summary>Field number for the "allPlyer" field.</summary>
    public const int AllPlyerFieldNumber = 1;
    private static readonly pb::FieldCodec<global::PlayerInfo.PlayerData> _repeated_allPlyer_codec
        = pb::FieldCodec.ForMessage(10, global::PlayerInfo.PlayerData.Parser);
    private readonly pbc::RepeatedField<global::PlayerInfo.PlayerData> allPlyer_ = new pbc::RepeatedField<global::PlayerInfo.PlayerData>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::PlayerInfo.PlayerData> AllPlyer {
      get { return allPlyer_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      allPlyer_.WriteTo(output, _repeated_allPlyer_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += allPlyer_.CalculateSize(_repeated_allPlyer_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            allPlyer_.AddEntriesFrom(input, _repeated_allPlyer_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
