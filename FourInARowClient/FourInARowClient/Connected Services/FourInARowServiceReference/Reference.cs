﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FourInARowClient.FourInARowServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserNameInUse", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
    [System.SerializableAttribute()]
    public partial class UserNameInUse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DetailsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Details {
            get {
                return this.DetailsField;
            }
            set {
                if ((object.ReferenceEquals(this.DetailsField, value) != true)) {
                    this.DetailsField = value;
                    this.RaisePropertyChanged("Details");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserConnectdFault", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
    [System.SerializableAttribute()]
    public partial class UserConnectdFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DetailsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Details {
            get {
                return this.DetailsField;
            }
            set {
                if ((object.ReferenceEquals(this.DetailsField, value) != true)) {
                    this.DetailsField = value;
                    this.RaisePropertyChanged("Details");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserNotRegisteredFault", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
    [System.SerializableAttribute()]
    public partial class UserNotRegisteredFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DetailsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Details {
            get {
                return this.DetailsField;
            }
            set {
                if ((object.ReferenceEquals(this.DetailsField, value) != true)) {
                    this.DetailsField = value;
                    this.RaisePropertyChanged("Details");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="IncorectPasswordFault", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
    [System.SerializableAttribute()]
    public partial class IncorectPasswordFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DetailsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Details {
            get {
                return this.DetailsField;
            }
            set {
                if ((object.ReferenceEquals(this.DetailsField, value) != true)) {
                    this.DetailsField = value;
                    this.RaisePropertyChanged("Details");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MoveResult", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
    public enum MoveResult : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        YouWon = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Draw = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotYourTurn = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        GameOn = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InvalidMove = 4,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OpponentDisconnectedFault", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
    [System.SerializableAttribute()]
    public partial class OpponentDisconnectedFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DetailsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Details {
            get {
                return this.DetailsField;
            }
            set {
                if ((object.ReferenceEquals(this.DetailsField, value) != true)) {
                    this.DetailsField = value;
                    this.RaisePropertyChanged("Details");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FourInARowServiceReference.IFourInARowService", CallbackContract=typeof(FourInARowClient.FourInARowServiceReference.IFourInARowServiceCallback))]
    public interface IFourInARowService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/Register", ReplyAction="http://tempuri.org/IFourInARowService/RegisterResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(FourInARowClient.FourInARowServiceReference.UserNameInUse), Action="http://tempuri.org/IFourInARowService/RegisterUserNameInUseFault", Name="UserNameInUse", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
        void Register(string userName, string hashedPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/Register", ReplyAction="http://tempuri.org/IFourInARowService/RegisterResponse")]
        System.Threading.Tasks.Task RegisterAsync(string userName, string hashedPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/ClientConnect", ReplyAction="http://tempuri.org/IFourInARowService/ClientConnectResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(FourInARowClient.FourInARowServiceReference.UserConnectdFault), Action="http://tempuri.org/IFourInARowService/ClientConnectUserConnectdFaultFault", Name="UserConnectdFault", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
        [System.ServiceModel.FaultContractAttribute(typeof(FourInARowClient.FourInARowServiceReference.UserNotRegisteredFault), Action="http://tempuri.org/IFourInARowService/ClientConnectUserNotRegisteredFaultFault", Name="UserNotRegisteredFault", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
        [System.ServiceModel.FaultContractAttribute(typeof(FourInARowClient.FourInARowServiceReference.IncorectPasswordFault), Action="http://tempuri.org/IFourInARowService/ClientConnectIncorectPasswordFaultFault", Name="IncorectPasswordFault", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
        void ClientConnect(string userName, string hashedPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/ClientConnect", ReplyAction="http://tempuri.org/IFourInARowService/ClientConnectResponse")]
        System.Threading.Tasks.Task ClientConnectAsync(string userName, string hashedPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/ReportMove", ReplyAction="http://tempuri.org/IFourInARowService/ReportMoveResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(FourInARowClient.FourInARowServiceReference.OpponentDisconnectedFault), Action="http://tempuri.org/IFourInARowService/ReportMoveOpponentDisconnectedFaultFault", Name="OpponentDisconnectedFault", Namespace="http://schemas.datacontract.org/2004/07/WcfFourInARowService")]
        FourInARowClient.FourInARowServiceReference.MoveResult ReportMove(int gameId, int location, int player);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/ReportMove", ReplyAction="http://tempuri.org/IFourInARowService/ReportMoveResponse")]
        System.Threading.Tasks.Task<FourInARowClient.FourInARowServiceReference.MoveResult> ReportMoveAsync(int gameId, int location, int player);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/StartNewGame", ReplyAction="http://tempuri.org/IFourInARowService/StartNewGameResponse")]
        void StartNewGame(string player1, string player2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/StartNewGame", ReplyAction="http://tempuri.org/IFourInARowService/StartNewGameResponse")]
        System.Threading.Tasks.Task StartNewGameAsync(string player1, string player2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/Disconnect", ReplyAction="http://tempuri.org/IFourInARowService/DisconnectResponse")]
        void Disconnect(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/Disconnect", ReplyAction="http://tempuri.org/IFourInARowService/DisconnectResponse")]
        System.Threading.Tasks.Task DisconnectAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/GetConnectedClients", ReplyAction="http://tempuri.org/IFourInARowService/GetConnectedClientsResponse")]
        System.Collections.Generic.Dictionary<string, object> GetConnectedClients();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFourInARowService/GetConnectedClients", ReplyAction="http://tempuri.org/IFourInARowService/GetConnectedClientsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, object>> GetConnectedClientsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFourInARowServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFourInARowService/sendGameInvitation")]
        void sendGameInvitation();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFourInARowService/AcceptGameInvitation")]
        void AcceptGameInvitation();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFourInARowService/OtherPlayerConnected")]
        void OtherPlayerConnected();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFourInARowService/OtherPlayerMoved")]
        void OtherPlayerMoved(FourInARowClient.FourInARowServiceReference.MoveResult moveResult, int location);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFourInARowServiceChannel : FourInARowClient.FourInARowServiceReference.IFourInARowService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FourInARowServiceClient : System.ServiceModel.DuplexClientBase<FourInARowClient.FourInARowServiceReference.IFourInARowService>, FourInARowClient.FourInARowServiceReference.IFourInARowService {
        
        public FourInARowServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public FourInARowServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public FourInARowServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public FourInARowServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public FourInARowServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Register(string userName, string hashedPassword) {
            base.Channel.Register(userName, hashedPassword);
        }
        
        public System.Threading.Tasks.Task RegisterAsync(string userName, string hashedPassword) {
            return base.Channel.RegisterAsync(userName, hashedPassword);
        }
        
        public void ClientConnect(string userName, string hashedPassword) {
            base.Channel.ClientConnect(userName, hashedPassword);
        }
        
        public System.Threading.Tasks.Task ClientConnectAsync(string userName, string hashedPassword) {
            return base.Channel.ClientConnectAsync(userName, hashedPassword);
        }
        
        public FourInARowClient.FourInARowServiceReference.MoveResult ReportMove(int gameId, int location, int player) {
            return base.Channel.ReportMove(gameId, location, player);
        }
        
        public System.Threading.Tasks.Task<FourInARowClient.FourInARowServiceReference.MoveResult> ReportMoveAsync(int gameId, int location, int player) {
            return base.Channel.ReportMoveAsync(gameId, location, player);
        }
        
        public void StartNewGame(string player1, string player2) {
            base.Channel.StartNewGame(player1, player2);
        }
        
        public System.Threading.Tasks.Task StartNewGameAsync(string player1, string player2) {
            return base.Channel.StartNewGameAsync(player1, player2);
        }
        
        public void Disconnect(string userName) {
            base.Channel.Disconnect(userName);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(string userName) {
            return base.Channel.DisconnectAsync(userName);
        }
        
        public System.Collections.Generic.Dictionary<string, object> GetConnectedClients() {
            return base.Channel.GetConnectedClients();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, object>> GetConnectedClientsAsync() {
            return base.Channel.GetConnectedClientsAsync();
        }
    }
}
