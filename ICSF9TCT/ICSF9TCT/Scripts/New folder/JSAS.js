//#region varaiables 
var loginDetails = {};
var logDetails = {};

var docNo = "0";
var docDate = "0";
var fgQty = 0;
var fgCode = 0;
var warehouse = "0";
var Narration =""; 


var item = 0;
var quantity =0;
var rate = 0;
var gross =0;
var partybatchno =0;
var ItemRemarks = "";
var OperatorName = "";
var MachineName = "";

var iDocDate = "0";
var logDetails = {};
var selectedRow = 1;
var requestIds = [];
var requestsProcessed = [];
var bodyRequestsProcessed = [];
var requestId = 0;
var validRows = 0;
var totalRows = 0;
var validRowsArray = [];
var noofRows = 0;
var rowData = {};
var tableData = [];
var sAbbr = "";
var bodyData = [];
var bodyRequests = [];
var BodyDataArr = [];
var vsBodyData = {};
var requestId = 1;
var requestsProcessed = [];
var AUTO_JVNO = 0;
var Exchange = 0;
var Location = 0;
var Cheque = 0;
var Card = 0;
var Pass = 0;
var Online = 0;
var Finance = 0;
var Wallet = 0;
var Warehouse = 0;
var Finance_Option = 0;
var dFinance = 0;
var ApprovedFinance = 0;
var hData = {};
var Cash = 0;
var BalanceRemarks = 0;
var baseUrl = 'http://localhost/F9ICSKaleGrp_As';
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


//Afer Save Function LIssPro dated 06-06-2022
function AfterSaveLIssPro(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveLIssPro", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveLIssPro(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,

            bvtype: logDetails.iVoucherType,
            bdocNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/LIssPro/LUpdateIssPro",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Afer Save Function PPCRMReq added by shaikh azhar (18-05-2022)
function AfterSavePPCRMReq(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSavePPCRMReq", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSavePPCRMReq(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/PPCPlan/UpdatePPCRMReq",
            
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Afer Save Function EINV (24-04-2022)
function AfterSaveEInv(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveEInv", ["", "DocNo", "Date", "sNarration", "FGCode", "FGQty", "Vendor Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveEInv(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/SalInv/UpdateSalInv",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);


                //alert(res.message);
                alert(r.data.message);
                //alert(r.message);

                // alert(r.status);

                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
        //console.log(e.message);
    }
}

//Afer Save Function Lastest GRNJW ( Removed the Vendor Annaxure Batch Consume)
function LAfterSaveGRNJW(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("LfieldValueCallbackIssueAfterSaveGRNJW", ["", "DocNo", "Date", "sNarration", "FGCode", "FGQty", "Vendor Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function LfieldValueCallbackIssueAfterSaveGRNJW(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/LGRNJW/LUpdateGRNJW",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);


                //alert(res.message);
                alert(r.data.message);
                //alert(r.message);

                // alert(r.status);

                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
        //console.log(e.message);
    }
}

//Afer Save Function RM Requisition By SFG Added By Rizwan 26-03-2022
function SFGRMReq2SFGReq(loginDetails, rowIndex) {
    //alert(loginDetails);	

    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueSFGRMReq2SFGReq", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueSFGRMReq2SFGReq(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/SFGRMReq/UpdateSFGReq",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });

    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)

    }
}
//Afer Save Function VenBtchCon Add By Shaikh Azhar 24-03-2022
function AfterSaveVenBtchCon(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveVenBtchCon", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveVenBtchCon(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/VenBtchCon/UpdateVenBtchCon",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Afer Save Function MRPRmReq Add By Shaikh Azhar 10-03-2022
function AfterSaveMRPRmReq(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveMRPRmReq", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveMRPRmReq(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/MRPRmReq/UpdateMRPRmReq",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}



//Afer Save Function CustVendCr Add By Shaikh Azhar 07-03-2022
function AfterSaveCustVendCr(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveCustVendCr", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveCustVendCr(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/CustVendCr/UpdateCustVendCr",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Afer Save Function SalvageCon Add By Shaikh Azhar 06-03-2022
function AfterSaveSalvageCon(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveSalvageCon", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveSalvageCon(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/SalvageCon/UpdateSalvageCon",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function CostBOM Add By Shaikh Azhar 05-03-2022
function AfterSaveCostBOM(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveCostBOM", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveCostBOM(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/CostBOM/UpdateCostBOM",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                //alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function FGLinRej Add By Shaikh Azhar 27-02-2022
function AfterSaveFGLinRej(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveFGLinRej", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveFGLinRej(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/FGLinRej/UpdateFGLinRej",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function MRPRmReq Add By Shaikh Azhar 20-02-2022
function AfterSaveMRPSfgReq(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveMRPSfgReq", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveMRPSfgReq(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/MRPSfgReq/UpdateMRPSfgReq",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function FGPlan Add By Shaikh Azhar 16-02-2022
function AfterSaveFGPlan(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveFGPlan", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveFGPlan(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/FGPlan/UpdateFGPlan",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}



//Afer Save Function DCRwkJW Add By Shaikh Azhar 29-01-2022
function AfterSaveDCRwkJW(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveDCRwkJW", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveDCRwkJW(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/DCRwkJW/UpdateDCRwkJW",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });

    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)

    }
}


//AfterSaveGRNImp Add By Rizwan 24-01-2021 Function
function AfterSaveGRNImp(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveGRNImp", ["", "DocNo", "Date", "sNarration", "Vendor Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveGRNImp(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/GRNRM/GRNImp2InQC",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function IssPro Add By Rizwan 13-01-2022
function AfterSaveIssProINQC(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveIssProINQC", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveIssProINQC(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/IssPro/UpdateIssProINQC",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });

    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)

    }
}

//Afer Save Function UpdateIssProTest Add By Azhar 05-06-2022
function AfterSaveIssProTest(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveIssProTest", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveIssProTest(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/IssPro/UpdateIssProTest",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });

    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)

    }
}
//Afer Save Function GRNJWPur Add By Rizwan 21-12-2021
function AfterSaveGRNJWPur(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveGRNJWPur", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveGRNJWPur(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/GRNJWPur/UpdateGRNJWPur",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });

    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
        
    }
}

//Afer Save Function SFGRMReq Added By Shaikh Azhar 08-01-2022
function AfterSaveSFGRMReq(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveSFGRMReq", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveSFGRMReq(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/SFGRMReq/UpdateSFGRMReq",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });

    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)

    }
}


//Afer Save Function RelPPCPlan
function AfterSaveRelProdOrd(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveRelProdOrd", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveRelProdOrd(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/RelProdOrd/UpdateRelProdOrd",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Afer Save Function PPCPlan Added By Azhar Dated 19-06-2022 for Changes L 
function LAfterSavePPCPlan(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("lfieldValueCallbackIssueAfterSavePPCPlan", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function lfieldValueCallbackIssueAfterSavePPCPlan(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/PPCPlan/LUpdatePPCPlan",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}



//Afer Save Function PPCPlan
function AfterSavePPCPlan(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSavePPCPlan", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSavePPCPlan(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/PPCPlan/UpdatePPCPlan",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function IssPro
function AfterSaveIssPro(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveIssPro", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveIssPro(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            
            bvtype: logDetails.iVoucherType,
            bdocNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/IssPro/UpdateIssPro",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Afer Save Function WorkOrd
function AfterSaveWorkOrd(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveWorkOrd", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveWorkOrd(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/WorkOrd/UpdateWorkOrd",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                if (r.data.message != "")
                {
                    alert(r.data.message);
                }
                
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Afer Save Function BOM
function AfterSaveCBOM(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveCBOM", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveCBOM(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/CBOM/UpdatePDoc",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function
function AfterSaveCENT(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveCENT", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveCENT(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;
        

        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/CENT/UpdateCBROD",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//New Aftersave for ProdnInwQC Add By Rizwan 16-01-2022
function AfterSaveProdnInwQC(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveProdnInwQC", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveProdnInwQC(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            //Change by Rizwan Order by Majid sir 25-01-2022
            //url: "/F9ICSKaleGrp_As/InQC/PostProQCTranf",
            url: "/F9ICSKaleGrp_As/ProdnInwQC/UpdateProdnInwQC",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}
//End

//Afer Save Function InQC
function AfterSaveInQC(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveInQC", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveInQC(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/InQC/UpdateInQC",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function
function AfterSaveQCGRNRM(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveQCGRNRM", ["", "DocNo", "Date",  "sNarration", "Vendor Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveQCGRNRM(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;
        

        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/GRNRM/UpdateQCGRNRM",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

function AfterSaveUpdateBatchGRNRM(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveUpdateBatchGRNRM", ["", "DocNo", "Date", "Warehouse", "sNarration", "Vendor Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveUpdateBatchGRNRM(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;
        warehouse = response.data[3].FieldValue;

        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/GRNRM/UpdateBatchGRNRM",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res=JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

function PEAfterSaves(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSave", ["", "DocNo", "Date","Warehouse", "sNarration","FGCode","FGQty","MachineName","OperatorName"],Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    
}

function fieldValueCallbackIssueAfterSave(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
	logDetails = response.data[0];
    docNo = response.data[1].FieldValue;
	docDate = response.data[2].FieldValue;
	warehouse = response.data[3].FieldValue;
        
	var data = {
	    CompanyId: logDetails.CompanyId,
	    SessionId: logDetails.SessionId,
	    LoginId: logDetails.LoginId,
	    vtype: logDetails.iVoucherType,
	    docNo: docNo,
	    BodyData: bodyData

	};

	console.log({ data })
	$.ajax({
	    type: "POST",
	    url: "/F9ICSKaleGrp_As/UpdateStockProduce/UpdateSP",
	    data: JSON.stringify(data),
	    contentType: "application/json; charset=utf-8",
	    async: false,
	    success: function (r) {
	        debugger
	        alert("Data Posted Succesfully")
	        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
	    },
	    error: function (e) {
	        console.log(e.message)
	        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
	    }

	});

        
    //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
	//Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
	

    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


function PEbodyCallbackAfterSave(response) {
    debugger;
    try {
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
        
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        console.log(response)
        debugger;
        validRows = response.RowsInfo.iValidRows;
        totalRows = response.RowsInfo.iTotalRows;
        bodyData = [];
        vsBodyData = {};
        BodyDataArr = [];

        //Focus8WAPI.getBodyFieldValue('PErowCallBack', ["", "*"], 2, false, 1, ++requestId);

        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/UpdateStockProduce/UpdateSP",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                alert("Data Posted Succesfully")
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });
    } catch (e) {
        console.log("bodyCallbackAfterSave", e.message)
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    }

}

function PErowCallBack(response) {
    console.log('PErowCallBack :: ', response);
    debugger;
    if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
        return;
    }
    requestsProcessed.push(response.iRequestId);
    validRows = response.RowsInfo.iValidRows;
    totalRows = response.RowsInfo.iTotalRows;
    bodyData = [];

    vsBodyData = {};
    vsBodyDataArr = [];

    bodyRequestsProcessed = [];
    for (let i = 1 ; i <= validRows; i++) {
        Focus8WAPI.getBodyFieldValue('PEinitializeRow', ["*"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, i, i);
    }
}

function PEinitializeRow(response) {

    try {
        if (isRequestCompleted(response.iRequestId, bodyRequestsProcessed)) {
            return;
        }
        bodyRequestsProcessed.push(response.iRequestId);

        const row = initializeRowDataFields(response.data.slice(0));
        bodyData.push(response.data.slice(0));
        vsBodyData[response.iRequestId] = row;

        //if (validRowsRMASales === Object.values(vsBodyData).length) {
        //    Focus8WAPI.getFieldValue("headerSalesCallbackalter", ["", "DocNo"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, ++requestId);
        //}
    } catch (e) {
        alert(e.message)
    }
}

//----------------------- End of P E    ------------------------------------------------------------//



function bodyCallbackAfterSave(response) {
    debugger;
    try {
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        console.log(response)
        item = response.data[0].FieldValue;
        partybatchno = response.data[1].FieldValue;
        ItemRemarks = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo
        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/RaiseFinanceJV/RaiseJV",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                alert("Data Posted Succesfully")
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });
    } catch (e) {
        console.log("bodyCallbackAfterSave", e.message)
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    }

}
//Before Delete Function GRNJW
function BeforeDeleteGRNJW(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueBeforeDeleteGRNJW", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueBeforeDeleteGRNJW(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/GRNJW/DeleteGRNJW",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Afer Save Function GRNJW
function AfterSaveGRNJW(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveGRNJW", ["", "DocNo", "Date", "sNarration", "FGCode", "FGQty", "Vendor Account"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false,requestId++);

}
function fieldValueCallbackIssueAfterSaveGRNJW(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;
        

        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/GRNJW/UpdateGRNJW",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);


                //alert(res.message);
                alert(r.data.message);
                //alert(r.message);

                // alert(r.status);

                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
        //console.log(e.message);
    }
}

//Afer Save Function DCJWSal
function AfterSaveDCJWSal(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveDCJWSal", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveDCJWSal(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;
        

        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/DCJWSal/UpdateDCJWSal",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);


                //alert(res.message);
                alert(r.data.message);
                //alert(r.message);

                // alert(r.status);

                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

// Added By Shaikh Azhar 29-01-2022 

//Before Delete Function DCJWSal
function BeforeDeleteDCJWSal(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueBeforeDeleteDCJWSal", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueBeforeDeleteDCJWSal(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/DCJWSal/DeleteDCJWSal",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);


                //alert(res.message);
                alert(r.data.message);
                //alert(r.message);

                // alert(r.status);

                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

/* Rakesh Works */
// Afer Save Function Store Issue Slip Added On [30-06-2022]

function AfterSaveSIS(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackSIS", ["", "*"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackSIS(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        docNo = response.data[1].FieldValue;
        docDate = response.data[2].FieldValue;


        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            User: logDetails.UserName,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            docNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/F9ICSKaleGrp_As/SIS/UpdateStock",

            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (r) {
                debugger
                res = JSON.stringify(r);
                alert(r.data.message);
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });


        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)
        //Focus8WAPI.getBodyFieldValue("PEbodyCallbackAfterSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)


    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}


//Before Save Function
function GINbeforeSave(logDetails, rowIndex) {
    debugger;
    requestId = 0;
    requestsProcessed = [];
    // logDetails = [...args][0];  
    alert(rowIndex);
    Focus8WAPI.getFieldValue("fieldValueCallbackBeforeSaveGIN", ["", "DocNo", "Date", "Vendor Account", "Branch", "Dept", "Work Center", "Gate Entry No", "Narration", "Gate Entry Status", "Gate Entry Type", "Gate Inward No", "DC No", "DC Date", "Vechicle No", "Driver Name", "Driver Mobile No", "Inward Ref No"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++)
}

function fieldValueCallbackBeforeSaveGIN(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        console.log(logDetails)
        docNo = response.data[1].FieldValue;
        console.log(docNo)
        Warehouse = response.data[2].FieldValue;
        console.log(Warehouse)
        idocDate = response.data[3].FieldValue;
        OperatorName = response.data[4].FieldValue;
        MachineName = response.data[5].FieldValue;
        Narration = response.data[6].FieldValue;
        

        //if (CheckbuyBack == 1) {

        Focus8WAPI.getBodyFieldValue("GINbodyCallbackBeforeSave", ["Item", "Quantity", "Rate", "Gross", "PartyBatchNo", "ItemRemarks"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, -1, requestId++)
        //}else
        //{
        //    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
        //}


    } catch (e) {
        console.log("headerCallback", e.message)
    }
}
function GINbodyCallbackBeforeSave(response) {
    debugger;
    try {
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        console.log(response)
        item = response.data[0].FieldValue;
        quantity = response.data[1].FieldValue;
        gross = response.data[2].FieldValue;
        partybatchno = response.data[3].FieldValue;
        ItemRemarks = response.data[4].FieldValue;
        debugger;
        //Finance = response.data[5].FieldValue;
        //Wallet = response.data[6].FieldValue;
        //sessionStorage.setItem("Cheque", Cheque);
        //sessionStorage.setItem("CompanyId", logDetails.CompanyId);
        //sessionStorage.setItem("iVoucherType", logDetails.iVoucherType);
        //sessionStorage.setItem("SessionId", logDetails.SessionId);
        //sessionStorage.setItem("LoginId", logDetails.LoginId);
        //sessionStorage.setItem("sAbbr", logDetails.sVoucherAbbreviation);
        //sessionStorage.setItem("docNo", docNo);
        //sessionStorage.setItem("Location", Location);
        //sessionStorage.setItem("Finance_Option", Finance_Option);
        //sessionStorage.setItem("idocDate", idocDate);
        //sessionStorage.setItem("CheckbuyBack", CheckbuyBack);

        //$('#hiddencheque').text(Cheque);
        //$('#hiddencheque').html(Cheque);
        //sessionStorage.setItem("Exchange", Exchange);
        //sessionStorage.setItem("Card", Card);
        //sessionStorage.setItem("Pass", Pass);
        //sessionStorage.setItem("Online", Online);
        //sessionStorage.setItem("Finance", Finance);
        //sessionStorage.setItem("Wallet", Wallet);
        //console.log(Exchange)

        //if (logDetails.sVoucherAbbreviation == 'RCO') {


        //    if (CheckbuyBack == 0 && Exchange == 0) {

        //        $('#id_transactionentry_header3_section').find('input').val('');
        //    }

        //    if (Online == 0) {

        //        $('#id_transactionentry_header4_section').find('input').val('');
        //    }

        //    if (Pass == 0) {

        //        $('#id_transactionentry_header5_section').find('input').val('');
        //    }
        //    if (Cheque == 0) {

        //        $('#id_transactionentry_header6_section').find('input').val('');
        //    }
        //    if (Card == 0) {

        //        $('#id_transactionentry_header7_section').find('input').val('');
        //    }
        //    if (Wallet == 0) {

        //        $('#id_transactionentry_header10_section').find('input').val('');
        //    }
        //}

        //if (logDetails.sVoucherAbbreviation == 'USL') {


        //    if (CheckbuyBack == 0 && Exchange == 0) {

        //        $('#id_transactionentry_header2_section').find('input').val('');
        //    }

        //    if (Online == 0) {

        //        $('#id_transactionentry_header3_section').find('input').val('');
        //    }

        //    if (Pass == 0) {

        //        $('#id_transactionentry_header4_section').find('input').val('');
        //    }
        //    if (Cheque == 0) {

        //        $('#id_transactionentry_header6_section').find('input').val('');
        //    }
        //    if (Card == 0) {

        //        $('#id_transactionentry_header7_section').find('input').val('');
        //    }
        //    if (Wallet == 0) {

        //        $('#id_transactionentry_header9_section').find('input').val('');
        //    }
        //}
        //if (logDetails.sVoucherAbbreviation == 'DSI' || logDetails.sVoucherAbbreviation == 'DTI') {


        //    if (CheckbuyBack == 0 && Exchange == 0) {

        //        $('#id_transactionentry_header3_section').find('input').val('');
        //    }

        //    if (Online == 0) {

        //        $('#id_transactionentry_header4_section').find('input').val('');
        //    }

        //    if (Pass == 0) {

        //        $('#id_transactionentry_header5_section').find('input').val('');
        //    }
        //    if (Cheque == 0) {

        //        $('#id_transactionentry_header7_section').find('input').val('');
        //    }
        //    if (Card == 0) {

        //        $('#id_transactionentry_header8_section').find('input').val('');
        //    }
        //    if (Wallet == 0) {

        //        $('#id_transactionentry_header10_section').find('input').val('');
        //    }
        //}
        //if (CheckbuyBack == 1 && Exchange == 0) {
        //    alert("Provide Exchange Footer Amount in Voucher")
        //}
        //else if (CheckbuyBack == 1 && Exchange > 0) {
        //    var url = baseUrl + `/BuyBack/GetBuyBackDetails?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&Exchange=${Exchange}&idocDate=${idocDate}`;
        //    console.log({ logDetails, docNo, url })
        //    Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
        //}
            //else if (Exchange == 0) {
            //    // $('.section_margin_top').find('input:text').val('');
            //    //var url = baseUrl + `/BuyBack/GetBuyBackDetails?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&Exchange=${Exchange}&idocDate=${idocDate}`;
            //    // $('#id_transactionentry_header3_section').empty();
            //}
        //else if (Cheque > 0) {
        //    var url = baseUrl + `/BuyBack/GetCheckRecieptDetails?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&Cheque=${Cheque}`;
        //    console.log({ logDetails, docNo, url })
        //    Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
        //}
        //else if (Card > 0) {

        //    var url = baseUrl + `/CardReceipt/GetCardRecieptDetails?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&Card=${Card}&Location=${Location}&idocDate=${idocDate}`;
        //    console.log({ logDetails, docNo, url })
        //    Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
        //}
        //else if (Pass > 0) {

        //    var url = baseUrl + `/PassReceipt/GetPassRecieptDetails?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&Pass=${Pass}&idocDate=${idocDate}`;
        //    console.log({ logDetails, docNo, url })
        //    Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
        //}
        //else if (Online > 0) {
        //    var url = baseUrl + `/OnlineReciept/GetOnlineRecieptDetails?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&Online=${Online}&idocDate=${idocDate}`;
        //    console.log({ logDetails, docNo, url })
        //    Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
        //}

        //else if (Wallet > 0) {
        //    var url = baseUrl + `/WalletReceipt/GetWalletRecieptDetails?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&Wallet=${Wallet}&idocDate=${idocDate}`;
        //    console.log({ logDetails, docNo, url })
        //    Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
            

        //}
        //else {
        //    Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
        //}
       // Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true );
    } catch (e) {
        console.log("bodyCallbackBeforeSave", e.message)
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    }

}

function loadFinance() {

    if (sessionStorage.getItem("Finance_Option") == 1 && parseFloat(sessionStorage.getItem("Finance")) > 0) {

        var url = baseUrl + `/FinanceReceipt/GetFinanceRecieptDetails?CompanyId=${sessionStorage.getItem("CompanyId")}&SessionId=${sessionStorage.getItem("SessionId")}&LoginId=${sessionStorage.getItem("LoginId")}&vtype=${sessionStorage.getItem("iVoucherType")}&docNo=${sessionStorage.getItem("docNo")}&Finance=${sessionStorage.getItem("Finance")}&idocDate=${sessionStorage.getItem("idocDate")}`;
        console.log({ logDetails, docNo, url })
        Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
    }
}
function loadCheque() {

    if (parseFloat(sessionStorage.getItem("Cheque")) > 0) {

        var url = baseUrl + `/BuyBack/GetCheckRecieptDetails?CompanyId=${sessionStorage.getItem("CompanyId")}&SessionId=${sessionStorage.getItem("SessionId")}&LoginId=${sessionStorage.getItem("LoginId")}&vtype=${sessionStorage.getItem("iVoucherType")}&docNo=${sessionStorage.getItem("docNo")}&Cheque=${sessionStorage.getItem("Cheque")}`;
        console.log({ logDetails, docNo, url })
        Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
    }
    else {
        loadPass();
    }
}
function loadCard() {

    if (parseFloat(sessionStorage.getItem("Card")) > 0) {

        var url = baseUrl + `/CardReceipt/GetCardRecieptDetails?CompanyId=${sessionStorage.getItem("CompanyId")}&SessionId=${sessionStorage.getItem("SessionId")}&LoginId=${sessionStorage.getItem("LoginId")}&vtype=${sessionStorage.getItem("iVoucherType")}&docNo=${sessionStorage.getItem("docNo")}&Card=${sessionStorage.getItem("Card")}&Location=${sessionStorage.getItem("Location")}&idocDate=${sessionStorage.getItem("idocDate")}`;
        console.log({ logDetails, docNo, url })
        Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
    }
 
    else
    {
        loadPass();
    }

}
function loadPass() {

    if (parseFloat(sessionStorage.getItem("Pass")) > 0) {

        var url = baseUrl + `/PassReceipt/GetPassRecieptDetails?CompanyId=${sessionStorage.getItem("CompanyId")}&SessionId=${sessionStorage.getItem("SessionId")}&LoginId=${sessionStorage.getItem("LoginId")}&vtype=${sessionStorage.getItem("iVoucherType")}&docNo=${sessionStorage.getItem("docNo")}&Pass=${sessionStorage.getItem("Pass")}&idocDate=${sessionStorage.getItem("idocDate")}`;
        console.log({ logDetails, docNo, url })
        Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
    }
    else {
        loadOnline();
    }
}
function loadOnline() {
    debugger
    if (parseFloat(sessionStorage.getItem("Online")) > 0) {

        var url = baseUrl + `/OnlineReciept/GetOnlineRecieptDetails?CompanyId=${sessionStorage.getItem("CompanyId")}&SessionId=${sessionStorage.getItem("SessionId")}&LoginId=${sessionStorage.getItem("LoginId")}&vtype=${sessionStorage.getItem("iVoucherType")}&docNo=${sessionStorage.getItem("docNo")}&Online=${sessionStorage.getItem("Online")}&idocDate=${sessionStorage.getItem("idocDate")}`;
        console.log({ logDetails, docNo, url })
        Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
    }
    else {
        loadWallet();
    }
}

function loadWallet() {

    if (parseFloat(sessionStorage.getItem("Wallet")) > 0) {

        var url = baseUrl + `/WalletReceipt/GetWalletRecieptDetails?CompanyId=${sessionStorage.getItem("CompanyId")}&SessionId=${sessionStorage.getItem("SessionId")}&LoginId=${sessionStorage.getItem("LoginId")}&vtype=${sessionStorage.getItem("iVoucherType")}&docNo=${sessionStorage.getItem("docNo")}&Wallet=${sessionStorage.getItem("Wallet")}&idocDate=${sessionStorage.getItem("idocDate")}`;
        console.log({ logDetails, docNo, url })
        Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);
    }
}
function openPopupCallBackBeforeSave(objWrapperResult) {
    try {
        debugger;
        console.log(objWrapperResult);
    }
    catch (err) {
        alert("Exception :: openPopupCallBackBeforeSave :" + err.message);
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
    }
}
//function RaiseJVAfterSave(...args) {
function RaiseJVAfterSave(logDetails, rowIndex) {

    Focus8WAPI.getFieldValue("HeaderCallbackData", ["", "*"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}

var HBuyBack = [];
var sIncrement = 0;

var sIncrementII = 0;

var sIncrementIII = 0;

var sIncrementIV = 0;

var sIncrementV = 0;

var remarks = null;
var receipt = [];
var Gross = 0;
var dNetAmt = 0;
function HeaderCallbackData(response) {
    debugger;
    try {

        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        debugger;
        logDetails = response.data[0];
        //console.log(logDetails)
        docNo = response.data[1].FieldValue;
        
        hData = initializeFields(response.data.slice(1));

        //remarks = hData["Balance_Remarks"]["FieldValue"];
  
        //BalanceRemarks = prompt('Enter MOP Remarks', remarks);

       
       //  Focus8WAPI.setFieldValue("setFieldCallback", ["Balance_Remarks"], [remarks], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);
      // Focus8WAPI.getBodyFieldValue("bodyCallbackAfterSaves", ["Exchange", "Cheque", "Card", "Pass", "Online", "Finance", "Wallet E-Pay", "Approved Finance", "Cash"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, -1, requestId++)

        Focus8WAPI.getBodyFieldValue("newbodyCallbackAfterSaves", ["Gross"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, 1, requestId++)



    } catch (e) {
        console.log("HeaderCallbackData", e.message)
    }
}


function setFieldCallback(res) {
    console.log(JSON.stringify(res));
}

function newbodyCallbackAfterSaves(response) {

    try {
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        //hData = initializeFields(response.data.slice(1));
     //   alert(2)
        console.log(response)
        Gross = response.data[0].FieldValue;
    Focus8WAPI.getBodyFieldValue("bodyCallbackAfterSaves", ["Exchange", "Cheque", "Card", "Pass", "Online", "Finance", "Wallet E-Pay", "Approved Finance", "Cash"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, -1, requestId++)


    
    
    } catch (e) {
        console.log("bodyCallbackAfterSaves", e.message)
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    }

}

function bodyCallbackAfterSaves(response) {

    try {
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        //hData = initializeFields(response.data.slice(1));

        sIncrement = "I";
        sIncrementII = "II";
        sIncrementIII = "III";
        sIncrementIV = "IV";
        sIncrementV = "V";

        Exchange = response.data[0].FieldValue;
        Cheque = response.data[1].FieldValue;
        Card = response.data[2].FieldValue;
        Pass = response.data[3].FieldValue;
        Online = response.data[4].FieldValue;
        Finance = response.data[5].FieldValue;
        Wallet = response.data[6].FieldValue;
        ApprovedFinance = response.data[7].FieldValue;
        Cash = response.data[8].FieldValue;

        if (Exchange == 0 && Cheque == 0 && Card == 0 && Pass == 0 && Online == 0 && Finance == 0 && Wallet == 0 && ApprovedFinance == 0 && Cash == 0) {
            console.log("MOP Amount is Zero")


            Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            return;
        }
        var FooterAmt = ( parseFloat(Exchange) + parseFloat(Cheque) + parseFloat(Card) +  parseFloat(Pass) +  parseFloat(Online)+  parseFloat(Finance) +  parseFloat(Wallet) +  parseFloat(ApprovedFinance) +  parseFloat(Cash));
        console.log(FooterAmt);
        dNetAmt = (parseFloat(Gross)) -(parseFloat(FooterAmt));
       // alert(dNetAmt)
        console.log(dNetAmt)
        if (dNetAmt > 0) {
            remarks = hData["Balance_Remarks"]["FieldValue"];
            //   remarks = "Good";
            BalanceRemarks = prompt('Enter MOP Remarks', remarks);

        }
        HBuyBack = [{
            Exchange: toNumber(response.data[0].FieldValue), Cheque: toNumber(response.data[1].FieldValue),

                Card: toNumber(response.data[2].FieldValue), Pass: toNumber(response.data[3].FieldValue),

                    Online: toNumber(response.data[4].FieldValue), Finance: toNumber(response.data[5].FieldValue),

                        Wallet: toNumber(response.data[6].FieldValue), ApprovedFinance: toNumber(response.data[7].FieldValue),
                Cash:toNumber(response.data[8].FieldValue)

        }
        ];
        console.log(HBuyBack)

        console.log(hData)
        receipt = [

            {

                Prod: hData["Prod" + sIncrement]["FieldValue"], ProdQty: hData["ProdQty" + sIncrement]["FieldValue"], Prod_Rate: hData["Prod_Rate" + sIncrement]["FieldValue"], ProdGross: hData["ProdGross" + sIncrement]["FieldValue"],
                 Cheque_Account: hData["Cheque_Account" + sIncrement]["FieldValue"], Cheque_Amt: hData["Cheque_Amt" + sIncrement]["FieldValue"],
                 Pass_Type: hData["Pass_Type" + sIncrement]["FieldValue"],PService_Charge: hData["PService_Charge" + sIncrement]["FieldValue"],
                 PGST: hData["PGST" + sIncrement]["FieldValue"],PDisc: hData["PDisc" + sIncrement]["FieldValue"],Pass_Amount: hData["Pass_Amount" + sIncrement]["FieldValue"],
                 Online_Type: hData["Online_Type" + sIncrement]["FieldValue"], Service_Charge: hData["Service_Charge" + sIncrement]["FieldValue"],
                 OnlineGST: hData["GST" + sIncrement]["FieldValue"], Online_Amt: hData["Online_Amt" + sIncrement]["FieldValue"],
                 EDC_Batch_ID: hData["EDC_Batch_ID" + sIncrement]["FieldValue"], MSF: hData["MSF" + sIncrement]["FieldValue"],
                 CardGST: hData["CRDGST" + sIncrement]["FieldValue"], Amt_Processed: hData["Amt_Processed" + sIncrement]["FieldValue"],
                 WType: hData["WType" + sIncrement]["FieldValue"], WSer_Charge: hData["WSer_Charge" + sIncrement]["FieldValue"],
                 WGST: hData["WGST" + sIncrement]["FieldValue"], WAmt: hData["WAmt" + sIncrement]["FieldValue"],
                 MSFAmount: hData["MSF" + sIncrement + "_Amount"]["FieldValue"], GSTAmount: hData["GST" + sIncrement + "_Amount"]["FieldValue"],
            },

        {
            Prod: hData["Prod" + sIncrementII]["FieldValue"], ProdQty: hData["ProdQty" + sIncrementII]["FieldValue"], Prod_Rate: hData["Prod_Rate" + sIncrementII]["FieldValue"], ProdGross: hData["ProdGross" + sIncrementII]["FieldValue"],
            Cheque_Account: hData["Cheque_Account" + sIncrementII]["FieldValue"], Cheque_Amt: hData["Cheque_Amt" + sIncrementII]["FieldValue"],
            Pass_Type: hData["Pass_Type" + sIncrementII]["FieldValue"], PService_Charge: hData["PService_Charge" + sIncrementII]["FieldValue"],
            PGST: hData["PGST" + sIncrementII]["FieldValue"], PDisc: hData["PDisc" + sIncrementII]["FieldValue"], Pass_Amount: hData["Pass_Amount" + sIncrementII]["FieldValue"],
            Online_Type: hData["Online_Type" + sIncrementII]["FieldValue"], Service_Charge: hData["Service_Charge" + sIncrementII]["FieldValue"],
            OnlineGST: hData["GST" + sIncrementII]["FieldValue"], Online_Amt: hData["Online_Amt" + sIncrementII]["FieldValue"],


            EDC_Batch_ID: hData["EDC_Batch_ID" + sIncrementII]["FieldValue"], MSF: hData["MSF" + sIncrementII]["FieldValue"],
            CardGST: hData["CRDGST" + sIncrementII]["FieldValue"], Amt_Processed: hData["Amt_Processed" + sIncrementII]["FieldValue"],
            WType: hData["WType" + sIncrementII]["FieldValue"], WSer_Charge: hData["WSer_Charge" + sIncrementII]["FieldValue"],
            WGST: hData["WGST" + sIncrementII]["FieldValue"], WAmt: hData["WAmt" + sIncrementII]["FieldValue"],
            MSFAmount: hData["MSF" + sIncrementII + "_Amount"]["FieldValue"], GSTAmount: hData["GST" + sIncrementII + "_Amount"]["FieldValue"],

        },

           {
               Prod: hData["Prod" + sIncrementIII]["FieldValue"], ProdQty: hData["ProdQty" + sIncrement]["FieldValue"], Prod_Rate: hData["Prod_Rate" + sIncrementIII]["FieldValue"], ProdGross: hData["ProdGross" + sIncrementIII]["FieldValue"],
               Cheque_Account: hData["Cheque_Account" + sIncrementIII]["FieldValue"], Cheque_Amt: hData["Cheque_Amt" + sIncrementIII]["FieldValue"],
               Pass_Type: hData["Pass_Type" + sIncrementIII]["FieldValue"], PService_Charge: hData["PService_Charge" + sIncrementIII]["FieldValue"],
               PGST: hData["PGST" + sIncrementIII]["FieldValue"], PDisc: hData["PDisc" + sIncrementIII]["FieldValue"], Pass_Amount: hData["Pass_Amount" + sIncrementIII]["FieldValue"],
               Online_Type: hData["Online_Type" + sIncrementIII]["FieldValue"], Service_Charge: hData["Service_Charge" + sIncrementIII]["FieldValue"],
               OnlineGST: hData["GST" + sIncrementIII]["FieldValue"], Online_Amt: hData["Online_Amt" + sIncrementIII]["FieldValue"],

               EDC_Batch_ID: hData["EDC_Batch_ID" + sIncrementIII]["FieldValue"], MSF: hData["MSF" + sIncrementIII]["FieldValue"],
               CardGST: hData["CRDGST" + sIncrementIII]["FieldValue"], Amt_Processed: hData["Amt_Processed" + sIncrementIII]["FieldValue"],
               WType: hData["WType" + sIncrementIII]["FieldValue"], WSer_Charge: hData["WSer_Charge" + sIncrementIII]["FieldValue"],
               WGST: hData["WGST" + sIncrementIII]["FieldValue"], WAmt: hData["WAmt" + sIncrementIII]["FieldValue"],
               MSFAmount: hData["MSF" + sIncrementIII + "_Amount"]["FieldValue"], GSTAmount: hData["GST" + sIncrementIII + "_Amount"]["FieldValue"],

           },
        {

        Prod: hData["Prod" + sIncrementIV]["FieldValue"], ProdQty: hData["ProdQty" + sIncrementIV]["FieldValue"], Prod_Rate: hData["Prod_Rate" + sIncrementIV]["FieldValue"], ProdGross: hData["ProdGross" + sIncrementIV]["FieldValue"],
        Cheque_Account: hData["Cheque_Account" + sIncrementIV]["FieldValue"], Cheque_Amt: hData["Cheque_Amt" + sIncrementIV]["FieldValue"],
        Pass_Type: hData["Pass_Type" + sIncrementIV]["FieldValue"],PService_Charge: hData["PService_Charge" + sIncrementIV]["FieldValue"],
        PGST: hData["PGST" + sIncrementIV]["FieldValue"],PDisc: hData["PDisc" + sIncrementIV]["FieldValue"],Pass_Amount: hData["Pass_Amount" + sIncrementIV]["FieldValue"],
        Online_Type: hData["Online_Type" + sIncrementIV]["FieldValue"], Service_Charge: hData["Service_Charge" + sIncrementIV]["FieldValue"],
        OnlineGST: hData["GST" + sIncrementIV]["FieldValue"], Online_Amt: hData["Online_Amt" + sIncrementIV]["FieldValue"],

        EDC_Batch_ID: hData["EDC_Batch_ID" + sIncrementIV]["FieldValue"], MSF: hData["MSF" + sIncrementIV]["FieldValue"],
        CardGST: hData["CRDGST" + sIncrementIV]["FieldValue"], Amt_Processed: hData["Amt_Processed" + sIncrementIV]["FieldValue"],
        WType: hData["WType" + sIncrementIV]["FieldValue"], WSer_Charge: hData["WSer_Charge" + sIncrementIV]["FieldValue"],
        WGST: hData["WGST" + sIncrementIV]["FieldValue"], WAmt: hData["WAmt" + sIncrementIV]["FieldValue"],
        MSFAmount: hData["MSF" + sIncrementIV + "_Amount"]["FieldValue"], GSTAmount: hData["GST" + sIncrementIV + "_Amount"]["FieldValue"],

        },
        {

            Prod: hData["Prod" + sIncrementV]["FieldValue"], ProdQty: hData["ProdQty" + sIncrementV]["FieldValue"], Prod_Rate: hData["Prod_Rate" + sIncrementV]["FieldValue"], ProdGross: hData["ProdGross" + sIncrementV]["FieldValue"],
            Cheque_Account: hData["Cheque_Account" + sIncrementV]["FieldValue"], Cheque_Amt: hData["Cheque_Amt" + sIncrementV]["FieldValue"],
            Pass_Type: hData["Pass_Type" + sIncrementV]["FieldValue"], PService_Charge: hData["PService_Charge" + sIncrementV]["FieldValue"],
            PGST: hData["PGST" + sIncrementV]["FieldValue"], PDisc: hData["PDisc" + sIncrementV]["FieldValue"], Pass_Amount: hData["Pass_Amount" + sIncrementV]["FieldValue"],
            Online_Type: hData["Online_Type" + sIncrementV]["FieldValue"], Service_Charge: hData["Service_Charge" + sIncrementV]["FieldValue"],
            OnlineGST: hData["GST" + sIncrementV]["FieldValue"], Online_Amt: hData["Online_Amt" + sIncrementV]["FieldValue"],

            EDC_Batch_ID: hData["EDC_Batch_ID" + sIncrementV]["FieldValue"], MSF: hData["MSF" + sIncrementV]["FieldValue"],
            CardGST: hData["CRDGST" + sIncrementV]["FieldValue"], Amt_Processed: hData["Amt_Processed" + sIncrementV]["FieldValue"],
            WType: hData["WType" + sIncrementV]["FieldValue"], WSer_Charge: hData["WSer_Charge" + sIncrementV]["FieldValue"],
            WGST: hData["WGST" + sIncrementV]["FieldValue"], WAmt: hData["WAmt" + sIncrementV]["FieldValue"],
            MSFAmount: hData["MSF" + sIncrementV + "_Amount"]["FieldValue"], GSTAmount: hData["GST" + sIncrementV + "_Amount"]["FieldValue"],

        }
        ];
        
        docNo = hData["DocNo"]["FieldValue"];
        Finance_Option = hData["Finance_Option"]["FieldValue"];
        CheckbuyBack = hData["Buy_Back"]["FieldValue"];
       // AUTO_JVNO = hData["AUTO_JVNO"]["FieldValue"];
        console.log(docNo)
        var data = {
            CompanyId: logDetails.CompanyId,
            SessionId: logDetails.SessionId,
            LoginId: logDetails.LoginId,
            vtype: logDetails.iVoucherType,
            iFATag: logDetails.iFATag,
            iInvTag:logDetails.iInvTag,
            docNo: docNo,
            Finance_Option: Finance_Option,
            receipt: receipt,
            HBuyBack: HBuyBack,
            Buy_Back: CheckbuyBack,
     BalanceRemarks,BalanceRemarks
        };
        $.ajax({
            type: "POST",
            url: baseUrl + "/RaiseFinanceJV/RaiseJV",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            async: false,
            //dataType: "json",
            success: function (data1) {
                debugger
                //alert("Data Posted Succesfully")
                if (data1.status) {
                    alert(data1.data.succes);             
                }

                else {
                    alert(data1.data.succes);

                }
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
            },
            error: function (e) {
                console.log(e.message)
                Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
            }

        });
    } catch (e) {
        console.log("bodyCallbackAfterSaves", e.message)
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    }

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
function initializeFields(fields) {
    let row = {};
    Object.values(fields).forEach((v, i, a) => {
        if (v) {

            row[`${v['sFieldName']}`] = {};
            row[`${v['sFieldName']}`]['sFieldName'] = v['sFieldName'];
            row[`${v['sFieldName']}`]['FieldText'] = v['FieldText'];
            row[`${v['sFieldName']}`]['FieldValue'] = v['FieldValue'];
            row[`${v['sFieldName']}`]['iFieldId'] = v['iFieldId'];
        }
    });
    return row;
}
function initializeRowCallback(response) {
    try {
        debugger
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        initializeFields(response.data);
        alert(10)

        console.log(response.data.Prod_RateI == 0)
        var prod1 = response.data.prod1;
        var prod2 = response.data.ProdQtyI;
        alert(prod1)
        //if (rows.length === validRows) {

        //    const isValidRows = rows.every(_=> _["MIN_No"].FieldValue && _["MIIR_No"].FieldValue)
        //    if (!isValidRows) {
        //        alert("MIIR No  is empty, Please check");
        //        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
        //    } else {
        //        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
        //    }
        //}
    } catch (e) {
        console.log("initializeRowCallback", e.message)
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    }
}
function initializeRowDataFields(fields) {
    debugger;
    try {
        var rowData = {};
        Object.values(fields).forEach((v, i, a) => {
            if (v) {
                rowData[`${v['sFieldName']}`] = {};
                rowData[`${v['sFieldName']}`]['sFieldName'] = v['sFieldName'];
                rowData[`${v['sFieldName']}`]['FieldText'] = v['FieldText'];
                rowData[`${v['sFieldName']}`]['FieldValue'] = v['FieldValue'];
                rowData[`${v['sFieldName']}`]['iFieldId'] = v['iFieldId'];
            }
        });
        rows.push(rowData);
    } catch (e) {

    }

    //console.log('rowData  string:: ', JSON.stringify(rowData));
    //console.log('rowData :: ', rowData)
}
function OldbodyCallbackAfterSaves(response) {

    try {
        if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
            return;
        }
        requestsProcessed.push(response.iRequestId);
        //hData = initializeFields(response.data.slice(1));

        BalanceRemarks = hData["Balance_Remarks"]["FieldValue"];

        Exchange = response.data[0].FieldValue;
        Cheque = response.data[1].FieldValue;
        Card = response.data[2].FieldValue;
        Pass = response.data[3].FieldValue;
        Online = response.data[4].FieldValue;
        Finance = response.data[5].FieldValue;
        Wallet = response.data[6].FieldValue;
        ApprovedFinance = response.data[7].FieldValue;
        Cash = response.data[8].FieldValue;

        console.log(Exchange);
        HBuyBack = [{
            Exchange: response.data[0].FieldValue, Cheque: response.data[1].FieldValue,

            Card: response.data[2].FieldValue, Pass: response.data[3].FieldValue,

            Online: response.data[4].FieldValue, Finance: response.data[5].FieldValue,

            Wallet: response.data[6].FieldValue, ApprovedFinance: response.data[7].FieldValue,
            Cash: response.data[8].FieldValue

        }
        ];
        var url = baseUrl + `/RaiseFinanceJV/ModeofRemarks?CompanyId=${logDetails.CompanyId}&SessionId=${logDetails.SessionId}&LoginId=${logDetails.LoginId}&vtype=${logDetails.iVoucherType}&docNo=${docNo}&BalanceRemarks=${BalanceRemarks}
        &iFATag=${logDetails.iFATag}&iInvTag=${logDetails.iInvTag}&Exchange=${Exchange}&Cheque=${Cheque}&Card=${Card}&Pass=${Pass}&Online=${Online}&Wallet=${Wallet}&Finance=${Finance}`;
        console.log({ logDetails, docNo, url })
        Focus8WAPI.openPopup(url, openPopupCallBackBeforeSave);

    } catch (e) {
        console.log("bodyCallbackAfterSaves", e.message)
        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false);
    }

}
