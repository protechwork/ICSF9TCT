//#region varaiables 
var loginDetails = {};
var logDetails = {};

var docNo = "0";
var docDate = "0";
var fgQty = 0;
var fgCode = 0;
var warehouse = "0";
var Narration = "";

var selectedRow = 1;
var requestIds = [];
var requestsProcessed = [];
var bodyRequestsProcessed = [];
var validRows = 0;
var totalRows = 0;
var validRowsArray = [];
var noofRows = 0;
var rowData = {};
var tableData = [];

var bodyData = [];
var bodyRequests = [];
var requestId = 1;
var requestsProcessed = [];


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

//#region Afer Save Function DCJWSal Dated 27-02-2024 
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
            url: "/ICSF9TCT/LDCJWSal/UpdateDCJWSal",
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
//#endregion

//Afer Save Function AnxJWSale dated 23-02-2023
function ASAnxJWSale(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveAnxJWSale", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveAnxJWSale(response) {
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
            docdate: docDate,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/ICSF9TCT/DCJWSal/UpdateAnxJWSale",
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

//Afer Save Function StoreIssue dated 06-06-2022 - Shaikh Azhar 
function AfterSaveStoreIssue(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveStoreIssue", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveStoreIssue(response) {
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
            url: "/ICSF9TCT/StoreIssue/UpdateStoreIssue",
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
            url: "/ICSF9TCT/RelProdOrd/UpdateRelProdOrd",
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

//Afer Save Function PendingIssProQC
function AfterSaveIssProQC(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveIssProQC", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveIssProQC(response) {
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
            idocDate: docDate,
            bdocNo: docNo,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/ICSF9TCT/LIssPro/PendingIssProQC",
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



//Afer Save Function ProdnPlan added by shaikh azhar (21-01-2023)
function AfterSaveProdnPlan(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveProdnPlan", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveProdnPlan(response) {
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
            url: "/ICSF9TCT/PPCPlan/UpdateProdnPlan",

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


//Afer Save Function ProdnPlan Added By Azhar Dated 21-01-2023 for Changes L 
function LAfterSaveProdnPlan(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("lfieldValueCallbackIssueAfterSaveProdnPlan", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function lfieldValueCallbackIssueAfterSaveProdnPlan(response) {
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
            url: "/ICSF9TCT/PPCPlan/LUpdateProdnPlan",
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

//Afer Save Function InQC @ 14-01-2023 
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
            url: "/ICSF9TCT/InwQCClear/UpdateInQC",
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

//Afer Save Function LIssPro dated 24-01-2023
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
            bdocdate: docDate,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/ICSF9TCT/LIssPro/LUpdateIssPro",
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



//Afer Save Function for change mng date to transtion date 06-03-2023
function ChnageMngToTranDate(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueChnageMngToTranDate", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueChnageMngToTranDate(response) {
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
            docdate: docDate,
            BodyData: bodyData

        };

        console.log({ data })
        $.ajax({
            type: "POST",
            url: "/ICSF9TCT/Generalized/ChangeMngToTrans",
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
    } catch (e) {
        console.log("fieldValueCallbacksupplierValidate", e.message)
    }
}

//Afer Save Function AfterSaveProdnPlanR1 added by Rakesh (26-04-2023) new
function AfterSaveProdnPlanR1(loginDetails, rowIndex) {
    //alert(loginDetails);	
    debugger;
    requestId = 0;
    requestsProcessed = [];
    Focus8WAPI.getFieldValue("fieldValueCallbackIssueAfterSaveProdnPlanR1", ["", "DocNo", "Date"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, requestId++);

}
function fieldValueCallbackIssueAfterSaveProdnPlanR1(response) {
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
            url: "/ICSF9TCT/PPCPlan/UpdateProdnPlanR1",

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
