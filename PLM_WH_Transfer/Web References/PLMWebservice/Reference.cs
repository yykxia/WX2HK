﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace PLM_WH_Transfer.PLMWebservice {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PLM_WebServiceSoap", Namespace="http://tempuri.org/")]
    public partial class PLM_WebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback offLineOperationCompleted;
        
        private System.Threading.SendOrPostCallback WH_Treat_flowLogin_isValidOperationCompleted;
        
        private System.Threading.SendOrPostCallback WH_queryOrderInfoByBarCodeOperationCompleted;
        
        private System.Threading.SendOrPostCallback isValidStorageCodeOperationCompleted;
        
        private System.Threading.SendOrPostCallback getDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback ExecuteDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback WH_queryOrderInfoByBarCode_TOperationCompleted;
        
        private System.Threading.SendOrPostCallback WH_queryOrderInfoByBarCode_SOperationCompleted;
        
        private System.Threading.SendOrPostCallback isBoundStateOperationCompleted;
        
        private System.Threading.SendOrPostCallback Storage_BoundStateOperationCompleted;
        
        private System.Threading.SendOrPostCallback StoragePosOperationCompleted;
        
        private System.Threading.SendOrPostCallback Transfer_offLineListOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PLM_WebService() {
            this.Url = global::PLM_WH_Transfer.Properties.Settings.Default.PLM_WH_Transfer_PLMWebservice_PLM_WebService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event offLineCompletedEventHandler offLineCompleted;
        
        /// <remarks/>
        public event WH_Treat_flowLogin_isValidCompletedEventHandler WH_Treat_flowLogin_isValidCompleted;
        
        /// <remarks/>
        public event WH_queryOrderInfoByBarCodeCompletedEventHandler WH_queryOrderInfoByBarCodeCompleted;
        
        /// <remarks/>
        public event isValidStorageCodeCompletedEventHandler isValidStorageCodeCompleted;
        
        /// <remarks/>
        public event getDataCompletedEventHandler getDataCompleted;
        
        /// <remarks/>
        public event ExecuteDataCompletedEventHandler ExecuteDataCompleted;
        
        /// <remarks/>
        public event WH_queryOrderInfoByBarCode_TCompletedEventHandler WH_queryOrderInfoByBarCode_TCompleted;
        
        /// <remarks/>
        public event WH_queryOrderInfoByBarCode_SCompletedEventHandler WH_queryOrderInfoByBarCode_SCompleted;
        
        /// <remarks/>
        public event isBoundStateCompletedEventHandler isBoundStateCompleted;
        
        /// <remarks/>
        public event Storage_BoundStateCompletedEventHandler Storage_BoundStateCompleted;
        
        /// <remarks/>
        public event StoragePosCompletedEventHandler StoragePosCompleted;
        
        /// <remarks/>
        public event Transfer_offLineListCompletedEventHandler Transfer_offLineListCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/offLine", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int offLine(ref System.DateTime optTime, string barCode, string optClient) {
            object[] results = this.Invoke("offLine", new object[] {
                        optTime,
                        barCode,
                        optClient});
            optTime = ((System.DateTime)(results[1]));
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void offLineAsync(System.DateTime optTime, string barCode, string optClient) {
            this.offLineAsync(optTime, barCode, optClient, null);
        }
        
        /// <remarks/>
        public void offLineAsync(System.DateTime optTime, string barCode, string optClient, object userState) {
            if ((this.offLineOperationCompleted == null)) {
                this.offLineOperationCompleted = new System.Threading.SendOrPostCallback(this.OnoffLineOperationCompleted);
            }
            this.InvokeAsync("offLine", new object[] {
                        optTime,
                        barCode,
                        optClient}, this.offLineOperationCompleted, userState);
        }
        
        private void OnoffLineOperationCompleted(object arg) {
            if ((this.offLineCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.offLineCompleted(this, new offLineCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/WH_Treat_flowLogin_isValid", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int WH_Treat_flowLogin_isValid(string loginClass, string loginGroup, string barCode, string loginType) {
            object[] results = this.Invoke("WH_Treat_flowLogin_isValid", new object[] {
                        loginClass,
                        loginGroup,
                        barCode,
                        loginType});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void WH_Treat_flowLogin_isValidAsync(string loginClass, string loginGroup, string barCode, string loginType) {
            this.WH_Treat_flowLogin_isValidAsync(loginClass, loginGroup, barCode, loginType, null);
        }
        
        /// <remarks/>
        public void WH_Treat_flowLogin_isValidAsync(string loginClass, string loginGroup, string barCode, string loginType, object userState) {
            if ((this.WH_Treat_flowLogin_isValidOperationCompleted == null)) {
                this.WH_Treat_flowLogin_isValidOperationCompleted = new System.Threading.SendOrPostCallback(this.OnWH_Treat_flowLogin_isValidOperationCompleted);
            }
            this.InvokeAsync("WH_Treat_flowLogin_isValid", new object[] {
                        loginClass,
                        loginGroup,
                        barCode,
                        loginType}, this.WH_Treat_flowLogin_isValidOperationCompleted, userState);
        }
        
        private void OnWH_Treat_flowLogin_isValidOperationCompleted(object arg) {
            if ((this.WH_Treat_flowLogin_isValidCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.WH_Treat_flowLogin_isValidCompleted(this, new WH_Treat_flowLogin_isValidCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/WH_queryOrderInfoByBarCode", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable WH_queryOrderInfoByBarCode(string barCode) {
            object[] results = this.Invoke("WH_queryOrderInfoByBarCode", new object[] {
                        barCode});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public void WH_queryOrderInfoByBarCodeAsync(string barCode) {
            this.WH_queryOrderInfoByBarCodeAsync(barCode, null);
        }
        
        /// <remarks/>
        public void WH_queryOrderInfoByBarCodeAsync(string barCode, object userState) {
            if ((this.WH_queryOrderInfoByBarCodeOperationCompleted == null)) {
                this.WH_queryOrderInfoByBarCodeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnWH_queryOrderInfoByBarCodeOperationCompleted);
            }
            this.InvokeAsync("WH_queryOrderInfoByBarCode", new object[] {
                        barCode}, this.WH_queryOrderInfoByBarCodeOperationCompleted, userState);
        }
        
        private void OnWH_queryOrderInfoByBarCodeOperationCompleted(object arg) {
            if ((this.WH_queryOrderInfoByBarCodeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.WH_queryOrderInfoByBarCodeCompleted(this, new WH_queryOrderInfoByBarCodeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/isValidStorageCode", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool isValidStorageCode(string barCode) {
            object[] results = this.Invoke("isValidStorageCode", new object[] {
                        barCode});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void isValidStorageCodeAsync(string barCode) {
            this.isValidStorageCodeAsync(barCode, null);
        }
        
        /// <remarks/>
        public void isValidStorageCodeAsync(string barCode, object userState) {
            if ((this.isValidStorageCodeOperationCompleted == null)) {
                this.isValidStorageCodeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnisValidStorageCodeOperationCompleted);
            }
            this.InvokeAsync("isValidStorageCode", new object[] {
                        barCode}, this.isValidStorageCodeOperationCompleted, userState);
        }
        
        private void OnisValidStorageCodeOperationCompleted(object arg) {
            if ((this.isValidStorageCodeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.isValidStorageCodeCompleted(this, new isValidStorageCodeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getData", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable getData(string sqlStr) {
            object[] results = this.Invoke("getData", new object[] {
                        sqlStr});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public void getDataAsync(string sqlStr) {
            this.getDataAsync(sqlStr, null);
        }
        
        /// <remarks/>
        public void getDataAsync(string sqlStr, object userState) {
            if ((this.getDataOperationCompleted == null)) {
                this.getDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDataOperationCompleted);
            }
            this.InvokeAsync("getData", new object[] {
                        sqlStr}, this.getDataOperationCompleted, userState);
        }
        
        private void OngetDataOperationCompleted(object arg) {
            if ((this.getDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDataCompleted(this, new getDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ExecuteData", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int ExecuteData(string sqlStr) {
            object[] results = this.Invoke("ExecuteData", new object[] {
                        sqlStr});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void ExecuteDataAsync(string sqlStr) {
            this.ExecuteDataAsync(sqlStr, null);
        }
        
        /// <remarks/>
        public void ExecuteDataAsync(string sqlStr, object userState) {
            if ((this.ExecuteDataOperationCompleted == null)) {
                this.ExecuteDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExecuteDataOperationCompleted);
            }
            this.InvokeAsync("ExecuteData", new object[] {
                        sqlStr}, this.ExecuteDataOperationCompleted, userState);
        }
        
        private void OnExecuteDataOperationCompleted(object arg) {
            if ((this.ExecuteDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExecuteDataCompleted(this, new ExecuteDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/WH_queryOrderInfoByBarCode_T", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable WH_queryOrderInfoByBarCode_T(string barCode) {
            object[] results = this.Invoke("WH_queryOrderInfoByBarCode_T", new object[] {
                        barCode});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public void WH_queryOrderInfoByBarCode_TAsync(string barCode) {
            this.WH_queryOrderInfoByBarCode_TAsync(barCode, null);
        }
        
        /// <remarks/>
        public void WH_queryOrderInfoByBarCode_TAsync(string barCode, object userState) {
            if ((this.WH_queryOrderInfoByBarCode_TOperationCompleted == null)) {
                this.WH_queryOrderInfoByBarCode_TOperationCompleted = new System.Threading.SendOrPostCallback(this.OnWH_queryOrderInfoByBarCode_TOperationCompleted);
            }
            this.InvokeAsync("WH_queryOrderInfoByBarCode_T", new object[] {
                        barCode}, this.WH_queryOrderInfoByBarCode_TOperationCompleted, userState);
        }
        
        private void OnWH_queryOrderInfoByBarCode_TOperationCompleted(object arg) {
            if ((this.WH_queryOrderInfoByBarCode_TCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.WH_queryOrderInfoByBarCode_TCompleted(this, new WH_queryOrderInfoByBarCode_TCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/WH_queryOrderInfoByBarCode_S", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable WH_queryOrderInfoByBarCode_S(string barCode) {
            object[] results = this.Invoke("WH_queryOrderInfoByBarCode_S", new object[] {
                        barCode});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public void WH_queryOrderInfoByBarCode_SAsync(string barCode) {
            this.WH_queryOrderInfoByBarCode_SAsync(barCode, null);
        }
        
        /// <remarks/>
        public void WH_queryOrderInfoByBarCode_SAsync(string barCode, object userState) {
            if ((this.WH_queryOrderInfoByBarCode_SOperationCompleted == null)) {
                this.WH_queryOrderInfoByBarCode_SOperationCompleted = new System.Threading.SendOrPostCallback(this.OnWH_queryOrderInfoByBarCode_SOperationCompleted);
            }
            this.InvokeAsync("WH_queryOrderInfoByBarCode_S", new object[] {
                        barCode}, this.WH_queryOrderInfoByBarCode_SOperationCompleted, userState);
        }
        
        private void OnWH_queryOrderInfoByBarCode_SOperationCompleted(object arg) {
            if ((this.WH_queryOrderInfoByBarCode_SCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.WH_queryOrderInfoByBarCode_SCompleted(this, new WH_queryOrderInfoByBarCode_SCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/isBoundState", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int isBoundState(string barCode) {
            object[] results = this.Invoke("isBoundState", new object[] {
                        barCode});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void isBoundStateAsync(string barCode) {
            this.isBoundStateAsync(barCode, null);
        }
        
        /// <remarks/>
        public void isBoundStateAsync(string barCode, object userState) {
            if ((this.isBoundStateOperationCompleted == null)) {
                this.isBoundStateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnisBoundStateOperationCompleted);
            }
            this.InvokeAsync("isBoundState", new object[] {
                        barCode}, this.isBoundStateOperationCompleted, userState);
        }
        
        private void OnisBoundStateOperationCompleted(object arg) {
            if ((this.isBoundStateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.isBoundStateCompleted(this, new isBoundStateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Storage_BoundState", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Storage_BoundState(string BarCode, string StorageCode, string RecordId, string BoundStatus, string onLineId, string BoundQty) {
            this.Invoke("Storage_BoundState", new object[] {
                        BarCode,
                        StorageCode,
                        RecordId,
                        BoundStatus,
                        onLineId,
                        BoundQty});
        }
        
        /// <remarks/>
        public void Storage_BoundStateAsync(string BarCode, string StorageCode, string RecordId, string BoundStatus, string onLineId, string BoundQty) {
            this.Storage_BoundStateAsync(BarCode, StorageCode, RecordId, BoundStatus, onLineId, BoundQty, null);
        }
        
        /// <remarks/>
        public void Storage_BoundStateAsync(string BarCode, string StorageCode, string RecordId, string BoundStatus, string onLineId, string BoundQty, object userState) {
            if ((this.Storage_BoundStateOperationCompleted == null)) {
                this.Storage_BoundStateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnStorage_BoundStateOperationCompleted);
            }
            this.InvokeAsync("Storage_BoundState", new object[] {
                        BarCode,
                        StorageCode,
                        RecordId,
                        BoundStatus,
                        onLineId,
                        BoundQty}, this.Storage_BoundStateOperationCompleted, userState);
        }
        
        private void OnStorage_BoundStateOperationCompleted(object arg) {
            if ((this.Storage_BoundStateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Storage_BoundStateCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/StoragePos", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void StoragePos(string storageCode, string optType) {
            this.Invoke("StoragePos", new object[] {
                        storageCode,
                        optType});
        }
        
        /// <remarks/>
        public void StoragePosAsync(string storageCode, string optType) {
            this.StoragePosAsync(storageCode, optType, null);
        }
        
        /// <remarks/>
        public void StoragePosAsync(string storageCode, string optType, object userState) {
            if ((this.StoragePosOperationCompleted == null)) {
                this.StoragePosOperationCompleted = new System.Threading.SendOrPostCallback(this.OnStoragePosOperationCompleted);
            }
            this.InvokeAsync("StoragePos", new object[] {
                        storageCode,
                        optType}, this.StoragePosOperationCompleted, userState);
        }
        
        private void OnStoragePosOperationCompleted(object arg) {
            if ((this.StoragePosCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.StoragePosCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Transfer_offLineList", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable Transfer_offLineList(string lineGroup) {
            object[] results = this.Invoke("Transfer_offLineList", new object[] {
                        lineGroup});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public void Transfer_offLineListAsync(string lineGroup) {
            this.Transfer_offLineListAsync(lineGroup, null);
        }
        
        /// <remarks/>
        public void Transfer_offLineListAsync(string lineGroup, object userState) {
            if ((this.Transfer_offLineListOperationCompleted == null)) {
                this.Transfer_offLineListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTransfer_offLineListOperationCompleted);
            }
            this.InvokeAsync("Transfer_offLineList", new object[] {
                        lineGroup}, this.Transfer_offLineListOperationCompleted, userState);
        }
        
        private void OnTransfer_offLineListOperationCompleted(object arg) {
            if ((this.Transfer_offLineListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Transfer_offLineListCompleted(this, new Transfer_offLineListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void offLineCompletedEventHandler(object sender, offLineCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class offLineCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal offLineCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public System.DateTime optTime {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.DateTime)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void WH_Treat_flowLogin_isValidCompletedEventHandler(object sender, WH_Treat_flowLogin_isValidCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class WH_Treat_flowLogin_isValidCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal WH_Treat_flowLogin_isValidCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void WH_queryOrderInfoByBarCodeCompletedEventHandler(object sender, WH_queryOrderInfoByBarCodeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class WH_queryOrderInfoByBarCodeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal WH_queryOrderInfoByBarCodeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataTable Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataTable)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void isValidStorageCodeCompletedEventHandler(object sender, isValidStorageCodeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class isValidStorageCodeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal isValidStorageCodeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void getDataCompletedEventHandler(object sender, getDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataTable Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataTable)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void ExecuteDataCompletedEventHandler(object sender, ExecuteDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExecuteDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExecuteDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void WH_queryOrderInfoByBarCode_TCompletedEventHandler(object sender, WH_queryOrderInfoByBarCode_TCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class WH_queryOrderInfoByBarCode_TCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal WH_queryOrderInfoByBarCode_TCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataTable Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataTable)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void WH_queryOrderInfoByBarCode_SCompletedEventHandler(object sender, WH_queryOrderInfoByBarCode_SCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class WH_queryOrderInfoByBarCode_SCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal WH_queryOrderInfoByBarCode_SCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataTable Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataTable)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void isBoundStateCompletedEventHandler(object sender, isBoundStateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class isBoundStateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal isBoundStateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void Storage_BoundStateCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void StoragePosCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    public delegate void Transfer_offLineListCompletedEventHandler(object sender, Transfer_offLineListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1098.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Transfer_offLineListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Transfer_offLineListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataTable Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataTable)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591