//#region varaiables 
var requestId = 1;
var CID = "";
var CID = ''; var SessID = ''; var UserName = '';
var logindetails = {};
var logDetails = {};
var ivouchertype = "0";
var headerarr = [];
var list = [];
var flag = 0;
var table = [];
var table1 = [];
var b_ArrRequest = [];
var ArrResponse = [];
var compid = [];
var vtype = [];
var row = 0;
var LoginId = 0;
var DocNo = 0;
var docDate = 0;
var vtyp=0;
var Branch = 0;
var Vendor = 0;
var reflist = "";
var Dept = 0;
var Warehouse = 0;
var QCItem = 0; 
var GRNNO =0;
var requestsProcessed = [];
//var requestsProcessed = [];
var sName = "";
var sCode = "";
var Item_d = 0;
var Item_qty =0;
var Focus8WAPI = {
    ENUMS: {
        MODULE_TYPE: {
            MASTER: 1,
            TRANSACTION: 2,
            UI: 3,
            GLOBAL: 4,
            MRP: 5,
            FixedAsset: 6,
            TransHome: 7
        },

        REQUEST_TYPE: {
            GET: 1,
            SET: 2,
            CONTINUE: 3,
            RESET_CACHE: 4
        },

        REQUEST_TYPE_UI: {
            SET_POPUP_COORDINATE: 1,
            OPEN_POPUP: 2,
            CLOSE_POPUP: 3,
            GOTOHOMEPAGE: 4,
            OPEN_INVOICE_DESIGNER: 5,
            AWAKE_SESSION: 6,
            LOGOUT: 7,
            MANDATORY_FIELDS_ENTRYSCREEN: 8
        }
    },

    getFieldValue: function (sCallbackFn, Field, iModuleType, isFieldId, iRequestId, bStruct) {
        var obj = null;

        try {
            obj = {
                moduleType: iModuleType,
                rowIndex: 0,
                isFieldId: isFieldId,
                requestType: Focus8WAPI.ENUMS.REQUEST_TYPE.GET,
                objData: { fieldid: Field },
                iRequestId: iRequestId,
                sCallbackFn: sCallbackFn,
                bStruct: bStruct
            };

            if (Focus8WAPI.PRIVATE.isValidInput(obj, false) == true) {
                Focus8WAPI.PRIVATE.postMessage(obj);
            }
        }
        catch (err) {
            alert("Exception: Focus8WAPI.getFieldValue " + err.message);
        }
    },

    setFieldValue: function (sCallbackFn, Field, Value, iModuleType, isFieldId, iRequestId, bStruct) {
        var obj = null;

        try {
            obj = {
                moduleType: iModuleType,
                rowIndex: 0,
                isFieldId: isFieldId,
                requestType: Focus8WAPI.ENUMS.REQUEST_TYPE.SET,
                objData: { fieldid: Field, value: Value },
                iRequestId: iRequestId,
                sCallbackFn: sCallbackFn,
                bStruct: bStruct
            };

            if (Focus8WAPI.PRIVATE.isValidInput(obj, false) == true) {
                Focus8WAPI.PRIVATE.postMessage(obj);
            }
        }
        catch (err) {
            alert("Exception: Focus8WAPI.setFieldValue " + err.message);
        }
    },

    getBodyFieldValue: function (sCallbackFn, Field, iModuleType, isFieldId, iRowIndex, iRequestId, bStruct) {
        var obj = null;

        try {
            obj = {
                moduleType: iModuleType,
                rowIndex: iRowIndex,
                isFieldId: isFieldId,
                requestType: Focus8WAPI.ENUMS.REQUEST_TYPE.GET,
                objData: { fieldid: Field },
                iRequestId: iRequestId,
                sCallbackFn: sCallbackFn,
                bStruct: bStruct
            };

            if (Focus8WAPI.PRIVATE.isValidInput(obj, true) == true) {
                Focus8WAPI.PRIVATE.postMessage(obj);
            }
        }
        catch (err) {
            alert("Exception: Focus8WAPI.getBodyFieldValue " + err.message);
        }
    },

    setBodyFieldValue: function (sCallbackFn, Field, Value, iModuleType, isFieldId, iRowIndex, iRequestId, bStruct) {
        var obj = null;

        try {
            obj = {
                moduleType: iModuleType,
                rowIndex: iRowIndex,
                isFieldId: isFieldId,
                requestType: Focus8WAPI.ENUMS.REQUEST_TYPE.SET,
                objData: { fieldid: Field, value: Value },
                iRequestId: iRequestId,
                sCallbackFn: sCallbackFn,
                bStruct: bStruct
            };

            if (Focus8WAPI.PRIVATE.isValidInput(obj, true) == true) {
                Focus8WAPI.PRIVATE.postMessage(obj);
            }
        }
        catch (err) {
            alert("Exception: Focus8WAPI.setBodyFieldValue " + err.message);
        }
    },

    continueModule: function (iModuleType, result) {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = iModuleType;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE.CONTINUE;
            obj.result = result;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.continueModule " + err.message);
        }
    },

    openPopup: function (url, sCallback) {
        var obj = null;

        try {
            if (Focus8WAPI.PRIVATE.isNullOrEmpty(url, true) == true) {
                return (false);
            }

            obj = {};
            obj.URL = url;
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.OPEN_POPUP;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.openPopup " + err.message);
        }

        return (true);
    },

    closePopup: function () {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.CLOSE_POPUP;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.closePopup " + err.message);
        }
    },

    gotoHomePage: function () {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.GOTOHOMEPAGE;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.gotoHomePage " + err.message);
        }
    },

    logout: function () {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.LOGOUT;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.logout " + err.message);
        }
    },

    awakeSession: function () {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.AWAKE_SESSION;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.awakeSession " + err.message);
        }
    },
    getMandatoryFields: function (sCallback, iMasterTypeId) {
        var obj = null;
        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.MASTER;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.MANDATORY_FIELDS_ENTRYSCREEN;
            obj.sCallbackFn = sCallback;
            obj.objData = iMasterTypeId;
            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.getMandatoryFields " + err.message);
        }
    },

    resetTransactionCache: function (iVoucherType) {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE.RESET_CACHE;
            obj.iVoucherType = iVoucherType;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.awakeSession " + err.message);
        }
    },

    setPopupCoordinates: function (sLeft, sTop, sWidth, sHeight) {
        var obj = null;
        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.SET_POPUP_COORDINATE;
            obj.Left = sLeft;
            obj.Top = sTop;
            obj.Width = sWidth;
            obj.Height = sHeight;
            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.openPopup " + err.message);
        }

        return (true);
    },

    getGlobalValue: function (sCallbackFn, sVariable, iRequestId) {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.GLOBAL;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE.GET;
            obj.Variable = sVariable;
            obj.iRequestId = iRequestId;
            obj.sCallbackFn = sCallbackFn;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.getGlobalValue " + err.message);
        }
    },

    openInvoiceDesigner: function (sCallbackFn, LayoutId, iVouchertype, iHeaderId, eModuleType, HeaderGroup, iSubReportId, bSaveHTMLSource, iRequestId) {
        var obj = null;
        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.OPEN_INVOICE_DESIGNER;
            obj.LayoutId = LayoutId;
            obj.iVouchertype = iVouchertype;
            obj.iHeaderId = iHeaderId;
            obj.ModuleType = eModuleType;
            obj.HeaderGroup = HeaderGroup;
            obj.iSubReportId = iSubReportId;
            obj.bSaveHTMLSource = bSaveHTMLSource;
            obj.sCallbackFn = sCallbackFn;
            obj.iRequestId = iRequestId;
            Focus8WAPI.PRIVATE.postMessage(obj);
            return obj;
        }
        catch (err) {
            alert("Exception: Focus8WAPI.openPopup " + err.message);
        }
    },

    PRIVATE: {
        isValidInput: function (obj, bBodyField) {
            try {
                if (Focus8WAPI.PRIVATE.isValidObject(obj.moduleType) == false || obj.moduleType.toString() == "") {
                    alert("Validation Exception: Please pass Module Type parameter");

                    return (false);
                }

                if (Focus8WAPI.PRIVATE.isValidObject(obj.isFieldId) == false || obj.isFieldId.toString() == "") {
                    alert("Validation Exception: Please pass isFieldId parameter");

                    return (false);
                }

                if (Focus8WAPI.PRIVATE.isValidObject(obj.objData.fieldid) == false) {
                    alert("Validation Exception: Please pass Field parameter");

                    return (false);
                }
                else {
                    if (Array.isArray(obj.objData.fieldid) == true) {
                        if (obj.objData.fieldid.length == 0) {
                            alert("Validation Exception: Please pass Field parameter");

                            return (false);
                        }
                    }
                }


                if (bBodyField == true) {
                    if (Focus8WAPI.PRIVATE.isValidObject(obj.rowIndex) == false) {
                        alert("Validation Exception: Row Index should be number type");

                        return (false);
                    }

                    if (Array.isArray(obj.rowIndex) == false) {
                        if (isNaN(obj.rowIndex)) {
                            alert("Validation Exception: Row Index should be number type");

                            return (false);
                        }

                        if (obj.rowIndex == 0) {
                            // alert("Validation Exception: Row Index should be greater than 0 for Body Fields");

                            return (false);
                        }
                    }
                }
            }
            catch (err) {
                alert("Exception: {Focus8WAPI.PRIVATE.isValidInput} " + err.message);
            }

            return (true);
        },

        postMessage: function (obj) {
            try {
                obj.FromClient = true;
                window.parent.postMessage(obj, "*");
            }
            catch (err) {
                // alert("Exception: Focus8WAPI.PRIVATE.postMessage " + err.message);
            }
        },

        onReceiveMessage: function (evt) {
            var objReturnData = null;
            var obj = null;

            try {
                Focus8WAPI.PRIVATE.stopKeyProcess(evt);
                objReturnData = evt.data;

                // Client                
                if (Focus8WAPI.PRIVATE.isValidObject(objReturnData.FromClient) == true) {
                    return;
                }

                console.log('Focus8WAPI::Received Response: ', JSON.stringify(objReturnData));

                if (Focus8WAPI.PRIVATE.isNullOrEmpty(objReturnData.sCallbackFn, true) == false) {
                    obj = {};
                    obj.returnCode = objReturnData.response.lValue;
                    obj.message = objReturnData.response.sValue;
                    obj.data = objReturnData.response.data;
                    obj.fieldId = objReturnData.fieldId;
                    obj.requestType = objReturnData.requestType;
                    obj.moduleType = objReturnData.moduleType;
                    obj.iRequestId = objReturnData.iRequestId;

                    if (Focus8WAPI.PRIVATE.isValidObject(objReturnData.RowsInfo) == true) {
                        obj.RowsInfo = objReturnData.RowsInfo;
                    }

                    eval(objReturnData.sCallbackFn)(obj);
                }
            }
            catch (err) {
                alert("Exception: Focus8WAPI.PRIVATE.onReceiveMessage " + err.message);
            }
        },

        isValidObject: function (obj) {
            try {
                if (typeof obj == "undefined" || obj == null) {
                    return (false);
                }

                return (true);
            }
            catch (err) {
                alert("Exception: {Focus8WAPI.PRIVATE.isValidObject} " + err.message);
            }

            return (false);
        },

        isNullOrEmpty: function (sValue, bTrim) {
            var bResult = false;

            try {
                if (Focus8WAPI.PRIVATE.isValidObject(sValue) == false || (typeof sValue).toLowerCase() != "string" || sValue.length <= 0) {
                    return (true);
                }

                if (Focus8WAPI.PRIVATE.isValidObject(bTrim) == true && bTrim == true) {
                    if (sValue.trim().length == 0) {
                        return (true);
                    }
                }
            }
            catch (err) {
                alert("Exception: {Focus8WAPI.PRIVATE.isNullOrEmpty} " + err.message);
                bResult = true;
            }

            return (bResult);
        },

        stopKeyProcess: function (evt) {
            try {
                if (Focus8WAPI.PRIVATE.isValidObject(evt) == false) {
                    return;
                }

                if (evt.preventDefault) {
                    evt.preventDefault();
                }
                else {
                    evt.returnValue = false;
                }

                if (evt.bubbles == true) {
                    evt.stopPropagation();
                }
            }
            catch (err) {
                alert("Exception: {Focus8WAPI.PRIVATE.stopKeyProcess} " + err.message);
            }
        }
    }

}
window.addEventListener('message', Focus8WAPI.PRIVATE.onReceiveMessage);


//#endregion

function isRequestCompleted(iRequestId, processedRequestsArray) {

    return processedRequestsArray.indexOf(iRequestId) === -1 ? false : true;

}

function isRequestProcessed(iRequestId) {
    for (let i = 0; i < requestsProcessed.length; i++) {
        if (requestsProcessed[i] == iRequestId) {
            return true;
        }
    } return false;
}

// Before Save DC Job Work Sales By Shaikh Azhar @ 24-02-2023 
function BSDCJWSal(logDetails, rowIndex) {
    debugger;
    
    CID = logDetails.CompanyId;
    SessID=logDetails.SessionId;
    UserName=logDetails.UserName;
    LoginId=logDetails.LoginId;
    //vtyp=logDetails.iVoucherType;
    // logDetails = [...args][0];  
    requestId=0;
    requestsProcessed = [];
    //Focus8WAPI.getFieldValue("PassSfieldValueCallback", ["", "DocNo", "Date", "Collection_Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
    //Focus8WAPI.getFieldValue("BSDCJWSalPassfieldValueCallback", ["DocNo", "Date", "CustomerAC", "Unit Location"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 2,true)
    Focus8WAPI.getFieldValue("BSDCJWSalPassfieldValueCallback", ["DocNo", "Date", "CustomerAC", "Unit Location"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 2)
    //Focus8WAPI.getFieldValue("BSDCJWSalPassfieldValueCallback", ["DocNo", "Date", "CustomerAC", "Unit Location"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true, ++requestId)
    
}
function BSDCJWSalPassfieldValueCallback(response) {

    try {

        /*if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);*/
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        //console.log(logDetails)
        debugger;
        docNo = response.data[0].FieldValue;
        idocDate = response.data[1].FieldValue;
        Vendor = response.data[2].FieldValue;
        Branch = response.data[3].FieldValue;
        

        console.log(docNo)

        debugger;

        //Focus8WAPI.getBodyFieldValue("BSDCJWSalPassSbodyCallbackBeforeSave", ["*"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, ++requestId)
        Focus8WAPI.getBodyFieldValue("BSDCJWSalPassSbodyCallbackBeforeSave", ["*"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false,1, 3,false)
        //if (Item != "") {
        //    var url = `/ICSF9TCT/DCJWSal/CheckAnxJWSale?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&User=${logDetails.UserName}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&docDate=${idocDate}Vendor=${Vendor}&Branch=${Branch}&Item=${Item}`;
        //    console.log({ logDetails, docNo, url })
        //    //Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
        //    $.ajax({
        //        type: "POST",
        //        url: url,
        //        contentType: "application/json; charset=utf-8",
        //        //   async: false,
        //        success: function (r) {
        //            debugger
        //            //res = JSON.stringify(r);
        //            //alert("dasda");
        //            Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
        //        },
        //        error: function (e) {
        //            console.log(e.message)
        //            Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
        //        }

        //    });

        //}

    } catch (e) {
        console.log("headerCallback", e.message)
    }
}
function BSDCJWSalPassSbodyCallbackBeforeSave(response) {
    debugger;

    try {
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);

        debugger;
        var rowInfo = response.RowsInfo;
        validRows = rowInfo.iValidRows;
        totalRows = rowInfo.iTotalRows;
        bodyRequestsProcessed = [];
        Item_d = response.data[2].FieldValue;  
        Item_qty = response.data[4].FieldValue;  
        rows = [];
        //if (vtyp!=0)
        // {
        vtyp=6150;
        // }
        debugger;
        if (Item_d != 0) {
            var url = `/ICSF9TCT/DCJWSal/CheckAnxJWSale?CompanyId=${CID}&SessionId=${SessID}&User=${UserName}&LoginId=${LoginId}&vtype=${vtyp}&docNo=${docNo}&docDate=${idocDate}&Vendor=${Vendor}&Branch=${Branch}&Item=${Item_d}&Item_qty=${Item_qty}`;
            console.log({ logDetails, docNo, url })
            //    //Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                //   async: false,
                success: function (r) {
                    debugger;
                    //res = JSON.stringify(r);
                    //alert("dasda");
                    //alert (r.message);
                    $("#id_body_33555725").val(r.data.message);
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
                },
                error: function (e) {
                    console.log(e.message)
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
                }

            });

        }


        //debugger;
        //alert (response.data[1].FieldValue);
        ////Pass = toMoney((Pass));

        //if (Branch != "") {
        //    var url =  `/ICSGeneral/PORM/BSPORM?CompanyId=${logDetails.CompanyId}&docdate=${idocDate}&Branch=${Branch}&User=${logDetails.UserName}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}`;
        //    console.log({ logDetails, Branch, url })
        //    Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
        //}

        //else {
        //    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            
        //}

    } catch (e) {
        console.log("bodyCallbackBeforeSave", e.message)
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    }

}

// Before Save IssPro
function ProdOrdAutoClose(logDetails, rowIndex) {
    debugger;

    // logDetails = [...args][0];  
    //Focus8WAPI.getFieldValue("PassSfieldValueCallback", ["", "DocNo", "RefList", "Date", "Collection_Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
    Focus8WAPI.getFieldValue("ProdOrdAutoClosePassfieldValueCallback", ["", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
}
function ProdOrdAutoClosePassfieldValueCallback(response) {

    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        //console.log(logDetails)
        debugger;
        idocDate = response.data[1].FieldValue;



        console.log(idocDate)

        debugger;

        //Focus8WAPI.getBodyFieldValue("PassSbodyCallbackBeforeSave", [""], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, ++requestId)
        if (idocDate != "") {
            var url = `/ICSF9TCT/LIssPro/ProdOrdAutoClose?CompanyId=${logDetails.CompanyId}&User=${logDetails.UserName}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&idocDate=${idocDate}`;
            console.log({ logDetails, idocDate, url })
            //Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                //   async: false,
                success: function (r) {
                    debugger
                    //res = JSON.stringify(r);
                    //alert("dasda");
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
                },
                error: function (e) {
                    console.log(e.message)
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
                }

            });

        }

    } catch (e) {
        console.log("headerCallback", e.message)
    }
}

// Before Save Gin
function GtEntRfPLst(logDetails, rowIndex) {
    debugger;

    // logDetails = [...args][0];  
    //Focus8WAPI.getFieldValue("PassSfieldValueCallback", ["", "DocNo", "RefList", "Date", "Collection_Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
    Focus8WAPI.getFieldValue("GtEntRfPLstPassfieldValueCallback", ["", "DocNo", "Date", "VendorAC", "Unit Location", "RefList"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
}
function GtEntRfPLstPassfieldValueCallback(response) {

    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        //console.log(logDetails)
        debugger;
        docNo = response.data[1].FieldValue;
        idocDate = response.data[2].FieldValue;
        Vendor = response.data[3].FieldValue;
        Branch = response.data[4].FieldValue;
        reflist = response.data[5].FieldValue;


        console.log(docNo)

        debugger;

        //Focus8WAPI.getBodyFieldValue("PassSbodyCallbackBeforeSave", [""], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, ++requestId)
        if (reflist != "") {
            var url = `/ICSF9TCT/GtEntRfPndngLst/GtEntRfPLst?CompanyId=${logDetails.CompanyId}&Branch=${Branch}&Vendor=${Vendor}&User=${logDetails.UserName}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&idocDate=${idocDate}&refLst=${reflist}`;
            console.log({ logDetails, docNo, url })
            //Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                //   async: false,
                success: function (r) {
                    debugger
                    //res = JSON.stringify(r);
                    //alert("dasda");
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
                },
                error: function (e) {
                    console.log(e.message)
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
                }

            });

        }

    } catch (e) {
        console.log("headerCallback", e.message)
    }
}
// Before Save InwQCClear
function GRNRMRfPLst(logDetails, rowIndex) {
    debugger;

    // logDetails = [...args][0];  
    //Focus8WAPI.getFieldValue("PassSfieldValueCallback", ["", "DocNo", "RefList", "Date", "Collection_Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
    Focus8WAPI.getFieldValue("GRNRMRfPLstPassfieldValueCallback", ["", "DocNo", "Date", "Department", "PartyName", "Warehouse", "QCItem"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
}
function GRNRMRfPLstPassfieldValueCallback(response) {

    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        //console.log(logDetails)
        debugger;
        docNo = response.data[1].FieldValue;
        idocDate = response.data[2].FieldValue;
        Dept = response.data[3].FieldValue;
        Vendor = response.data[4].FieldValue;
        Warehouse = response.data[5].FieldValue;
        QCItem = response.data[6].FieldValue;
        console.log(docNo)

        debugger;

        //Focus8WAPI.getBodyFieldValue("PassSbodyCallbackBeforeSave", [""], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, ++requestId)
        if (Dept != "") {
            var url = `/ICSF9TCT/InwQCClear/BS_PopulteGRNNO?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&User=${logDetails.UserName}&LoginId=${logDetails.LoginId}&bvtype=${logDetails.iVoucherType}&bdocNo=${docNo}&idocDate=${idocDate}&Dept=${Dept}&Vendor=${Vendor}&Warehouse=${Warehouse}&QCItem=${QCItem}`;
            console.log({ logDetails, docNo, url })
            //Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                //   async: false,
                success: function (r) {
                    debugger
                    //res = JSON.stringify(r);
                    //alert("dasda");
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
                },
                error: function (e) {
                    console.log(e.message)
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
                }

            });

        }

    } catch (e) {
        console.log("headerCallback", e.message)
    }
}
function GRNRMOtherFlds(logDetails, rowIndex) {
    debugger;

    // logDetails = [...args][0];  
    //Focus8WAPI.getFieldValue("PassSfieldValueCallback", ["", "DocNo", "RefList", "Date", "Collection_Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
    Focus8WAPI.getFieldValue("GRNRMOtherFldsPassfieldValueCallback", ["", "DocNo", "Date", "Department", "PartyName", "Warehouse", "QCItem","GRNNO"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
}
function GRNRMOtherFldsPassfieldValueCallback(response) {

    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        //console.log(logDetails)
        debugger;
        docNo = response.data[1].FieldValue;
        idocDate = response.data[2].FieldValue;
        Dept = response.data[3].FieldValue;
        Vendor = response.data[4].FieldValue;
        Warehouse = response.data[5].FieldValue;
        QCItem = response.data[6].FieldValue;
        GRNNO = response.data[7].FieldValue;
        console.log(docNo)

        debugger;

        //Focus8WAPI.getBodyFieldValue("PassSbodyCallbackBeforeSave", [""], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, ++requestId)
        if (Dept != "") {
            var url = `/ICSF9TCT/InwQCClear/BS_PopulteOtherFlds?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&User=${logDetails.UserName}&LoginId=${logDetails.LoginId}&bvtype=${logDetails.iVoucherType}&bdocNo=${docNo}&idocDate=${idocDate}&Dept=${Dept}&Vendor=${Vendor}&Warehouse=${Warehouse}&QCItem=${QCItem}&iBodyId=${GRNNO}`;
            console.log({ logDetails, docNo, url })
            //Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                //   async: false,
                success: function (r) {
                    debugger
                    //res = JSON.stringify(r);
                    //res =JSON.stringify(r.data.succes);
                    // const myObj = JSON.parse(res);
                    //var result = myObj.split(',');
                    document.getElementById("id_header_67110094").value = r.data.succes[0];
                    document.getElementById("id_header_67110093").value = r.data.succes[2];
                    document.getElementById("id_header_67110141").value = r.data.succes[3];
                    document.getElementById("id_header_67110142").value = r.data.succes[5];
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
                },
                error: function (e) {
                    console.log(e.message)
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
                }

            });

        }

    } catch (e) {
        console.log("headerCallback", e.message)
    }
}

//#region Before Save For Get Max FG Qty For RecoItem
function GetMaxFGLst(logDetails, rowIndex) {
    Focus8WAPI.getFieldValue("GetMaxFGLstPassfieldValueCallback", ["", "DocNo", "Date", "CustomerAC", "Unit Location", "DCRecoItem","TotalDCQty"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
    //Focus8WAPI.getFieldValue("GetMaxFGLstPassfieldValueCallback", ["", "*"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId)
    debugger;
}
function GetMaxFGLstPassfieldValueCallback(response) {

    try {
       
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        
         console.log(logDetails)
        debugger;
        DocNo = response.data[1].FieldValue;  
        iDocDate = response.data[2].FieldValue;
        Vendor = response.data[3].FieldValue;
        UnitLocation = response.data[4].FieldValue;
        item= response.data[5].FieldValue;
        DCQty = response.data[6].FieldValue;
        //CID = response.data[0].CompanyId;
        //SessID = response.data[0].SessionId;
        //UserName = response.data[0].CompanyId;
        debugger;
        console.log(DocNo);

        //debugger;

        //Focus8WAPI.getBodyFieldValue("PassSbodyCallbackBeforeSave", [""], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, ++requestId)
        if (DCQty >0) {
        
            var url = `/ICSF9TCT/LDCJWSal/GetMaxFGLst?CompanyId=${logDetails.CompanyId}&item=${item}&UnitLocation=${UnitLocation}&Vendor=${Vendor}&User=${logDetails.UserName}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&DocNo=${DocNo}&iDocDate=${iDocDate}&DCQty=${DCQty}`;
            console.log({ logDetails, DocNo, url })
            //alert(url);
            debugger;
            //Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                //   async: false,
                success: function (r) {
                    debugger;
                    //res = JSON.stringify(r);//

                    // start exporting notepad
                    res = JSON.stringify(r);
                    if (r.data.message == "")
                    {
                        alert("Posted Successfully");
                    }
                    else
                    {                    
                        //Convert Byte Array to BLOB.
                        var blob = new Blob([r.data.json_data], { type: "application/octetstream" });


                        //Check the Browser type and download the File.
                        var isIE = false || !!document.documentMode;
                        if (isIE) {
                            window.navigator.msSaveBlob(blob, docNo + "-" + new Date().getHours()+ "-" +  new Date().getMinutes() + "-Einvoice.json");
                        } else {
                            var url = window.URL || window.webkitURL;
                            link = url.createObjectURL(blob);
                            var a = $("<a />");                        
                            a.attr("download", DocNo + ".txt");
                            a.attr("href", link);
                            $("body").append(a);
                            a[0].click();
                            $("body").remove(a);
                        }
                    }
                    
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
                },
                error: function (e) {
                    console.log(e.message)
                    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
                }

            });

        }

    } catch (e) {
        console.log("headerCallback", e.message)
    }
}
//#endregion


function toMoney(value, format = "en-IN", options = { minimumFractionDigits: 2, maximumFractionDigits: 2 }) {
    value = parseFloat(value);
    if (isNaN(value))
        return '';
    return Number(value).toLocaleString(format, options);
}

    function toNumber(value) {
        if (!value) {
            return 0;
        }
        if (!isNaN(value)) {
            return value ? parseFloat(value) : 0;
        }
        return parseFloat(value.split(',').join(''));
    }

    function openPopupCallBackBeforeSave(objWrapperResult) {
        try {

            console.log(objWrapperResult);
        }
        catch (err) {
            alert("Exception :: openPopupCallBackBeforeSave :" + err.message);
            Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
        }
    }