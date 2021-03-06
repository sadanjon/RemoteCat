// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Common.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace RdpBridge.Protocol {

  /// <summary>Holder for reflection information generated from Common.proto</summary>
  public static partial class CommonReflection {

    #region Descriptor
    /// <summary>File descriptor for Common.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommonReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgxDb21tb24ucHJvdG8SBkNvbW1vbiJQCg5TZXNzaW9uTWVzc2FnZRIzCgxz",
            "ZXNzaW9uU3RhcnQYASABKAsyGy5Db21tb24uU2Vzc2lvblN0YXJ0TWVzc2Fn",
            "ZUgAQgkKB21lc3NhZ2UiKAoTU2Vzc2lvblN0YXJ0TWVzc2FnZRIRCglzZXNz",
            "aW9uSWQYASABKAlCFaoCElJkcEJyaWRnZS5Qcm90b2NvbGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::RdpBridge.Protocol.SessionMessage), global::RdpBridge.Protocol.SessionMessage.Parser, new[]{ "SessionStart" }, new[]{ "Message" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::RdpBridge.Protocol.SessionStartMessage), global::RdpBridge.Protocol.SessionStartMessage.Parser, new[]{ "SessionId" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class SessionMessage : pb::IMessage<SessionMessage>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SessionMessage> _parser = new pb::MessageParser<SessionMessage>(() => new SessionMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SessionMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RdpBridge.Protocol.CommonReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SessionMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SessionMessage(SessionMessage other) : this() {
      switch (other.MessageCase) {
        case MessageOneofCase.SessionStart:
          SessionStart = other.SessionStart.Clone();
          break;
      }

      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SessionMessage Clone() {
      return new SessionMessage(this);
    }

    /// <summary>Field number for the "sessionStart" field.</summary>
    public const int SessionStartFieldNumber = 1;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::RdpBridge.Protocol.SessionStartMessage SessionStart {
      get { return messageCase_ == MessageOneofCase.SessionStart ? (global::RdpBridge.Protocol.SessionStartMessage) message_ : null; }
      set {
        message_ = value;
        messageCase_ = value == null ? MessageOneofCase.None : MessageOneofCase.SessionStart;
      }
    }

    private object message_;
    /// <summary>Enum of possible cases for the "message" oneof.</summary>
    public enum MessageOneofCase {
      None = 0,
      SessionStart = 1,
    }
    private MessageOneofCase messageCase_ = MessageOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MessageOneofCase MessageCase {
      get { return messageCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMessage() {
      messageCase_ = MessageOneofCase.None;
      message_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SessionMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SessionMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(SessionStart, other.SessionStart)) return false;
      if (MessageCase != other.MessageCase) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (messageCase_ == MessageOneofCase.SessionStart) hash ^= SessionStart.GetHashCode();
      hash ^= (int) messageCase_;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (messageCase_ == MessageOneofCase.SessionStart) {
        output.WriteRawTag(10);
        output.WriteMessage(SessionStart);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (messageCase_ == MessageOneofCase.SessionStart) {
        output.WriteRawTag(10);
        output.WriteMessage(SessionStart);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (messageCase_ == MessageOneofCase.SessionStart) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(SessionStart);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SessionMessage other) {
      if (other == null) {
        return;
      }
      switch (other.MessageCase) {
        case MessageOneofCase.SessionStart:
          if (SessionStart == null) {
            SessionStart = new global::RdpBridge.Protocol.SessionStartMessage();
          }
          SessionStart.MergeFrom(other.SessionStart);
          break;
      }

      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            global::RdpBridge.Protocol.SessionStartMessage subBuilder = new global::RdpBridge.Protocol.SessionStartMessage();
            if (messageCase_ == MessageOneofCase.SessionStart) {
              subBuilder.MergeFrom(SessionStart);
            }
            input.ReadMessage(subBuilder);
            SessionStart = subBuilder;
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            global::RdpBridge.Protocol.SessionStartMessage subBuilder = new global::RdpBridge.Protocol.SessionStartMessage();
            if (messageCase_ == MessageOneofCase.SessionStart) {
              subBuilder.MergeFrom(SessionStart);
            }
            input.ReadMessage(subBuilder);
            SessionStart = subBuilder;
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class SessionStartMessage : pb::IMessage<SessionStartMessage>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SessionStartMessage> _parser = new pb::MessageParser<SessionStartMessage>(() => new SessionStartMessage());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SessionStartMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RdpBridge.Protocol.CommonReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SessionStartMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SessionStartMessage(SessionStartMessage other) : this() {
      sessionId_ = other.sessionId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SessionStartMessage Clone() {
      return new SessionStartMessage(this);
    }

    /// <summary>Field number for the "sessionId" field.</summary>
    public const int SessionIdFieldNumber = 1;
    private string sessionId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string SessionId {
      get { return sessionId_; }
      set {
        sessionId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SessionStartMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SessionStartMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SessionId != other.SessionId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (SessionId.Length != 0) hash ^= SessionId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (SessionId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(SessionId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (SessionId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(SessionId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (SessionId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SessionId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SessionStartMessage other) {
      if (other == null) {
        return;
      }
      if (other.SessionId.Length != 0) {
        SessionId = other.SessionId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            SessionId = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            SessionId = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
