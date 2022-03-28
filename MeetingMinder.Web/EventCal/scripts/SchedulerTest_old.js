var currentUpdateEvent;
var addStartDate;
var addEndDate;
var globalAllDay;

function updateEvent(event, element) {
    //alert(event.description);
    debugger;
    if ($(this).data("qtip")) $(this).qtip("destroy");

    currentUpdateEvent = event;

    //$('#updatedialog').dialog('open');
    $('#addDialog').dialog('open');
    debugger;
    $('#tblUpdate').attr("style", "display:block");
    $('#tblAdd').attr("style", "display:none");

    $("#txtNameOfVisitorUpdate").val(event.nameOfVisitor);
    $("#txtDesignOfVisitorUpdate").val(event.designtionOfVisitor);
    $("#txtNameOfVisitorOrgUpdate").val(event.nameOfVisitorOrganisation);
    $("#txtAppointmentUpdate").val(event.appointment);
    $("#txtNameDesignationUpdate").val(event.nameDesignation);
    $("#fileUploadPhotoUpdate").val(event.uploadPhoto);
    $("#fileUploadAttachmentsUpdate").val(event.attachments);
    $("#txtInformationUpdate").val(event.information);
    $("#eventId").val(event.id);
    $("#eventStartUpdate").text("" + event.start.toLocaleString());

    $("#eventStartUpdateView").text("" + event.start.format("dd/MM/yyyy"));
    $("#hdnEventStartUpdate").val("" + event.start.format("MM/dd/yyyy"));
    $("#ddlStartTimeUp").val("" + event.start.format("HH:mm:00"));

    if (event.end === null) {
        $("#eventEnd").text("");
    }
    else {
        $("#eventEndView").text("" + event.end.format("dd/MM/yyyy"));
        $("#eventEnd").text("" + event.end.toLocaleString());
        $("#ddlEndUp").val("" + event.end.format("HH:mm:00"));
        $("#hdnEventEndUpdate").val("" + event.start.format("MM/dd/yyyy"));
    }

}

function updateSuccess(updateResult) {
    //alert(updateResult);
}

function deleteSuccess(deleteResult) {
    //alert(deleteResult);
}

function addSuccess(addResult) {
    // if addresult is -1, means event was not added
    //    alert("added key: " + addResult);
    debugger;
    if (addResult != -1) {
        $('#calendar').fullCalendar('renderEvent',
						{
						    title: $("#txtAddNameOfVisitor").val(),
						    nameOfVisitor: $("#txtAddNameOfVisitor").val(),
						    designtionOfVisitor: $("#txtAddDesignOfVisitor").val(),
						    nameOfVisitorOrganisation: $("#txtAddNameOfVisitorOrg").val(),
						    appointment: $("#txtAddAppointment").val(),
						    nameDesignation: $("#txtAddNameDesignation").val(),
						    uploadPhoto: "test",//$("#fileUploadPhotoAdd").val(),
						    attachments: "ters",//$("#fileUploadAttachmentsAdd").val(),
						    information: $("#AddtxtInformation").val(),
						    start: addStartDate,
						    end: addEndDate,
						    id: addResult,
						    allDay: globalAllDay
						},
						true // make the event "stick"
					);


        $('#calendar').fullCalendar('unselect');
    }

}

function UpdateTimeSuccess(updateResult) {
    //alert(updateResult);
    debugger;
}


function selectDate(start, end, allDay) {
    debugger;
    $('#addDialog').dialog('open');

    $('#tblUpdate').attr("style", "display:none");
    $('#tblAdd').attr("style", "display:block");

    $("#txtAddNameOfVisitor").val("");
    $("#txtAddNameOfVisitor").val("");
    $("#txtAddDesignOfVisitor").val("");
    $("#txtAddNameOfVisitorOrg").val("");
    $("#txtAddAppointment").val("");
    $("#txtAddNameDesignation").val("");
    $("#fileUploadPhotoAdd").val();
    $("#fileUploadAttachmentsAdd").val();
    $("#AddtxtInformation").val("");

    $("#addEventStartDate").text("" + start.format("dd/MM/yyyy")); //start.toLocaleString());

    $("#addEventEndDate").text("" + end.format("dd/MM/yyyy")); //end.toLocaleString());

    $("#hdnEventStartDate").val("" + start.format("MM/dd/yyyy")); //start.toLocaleString());

    $("#hdnEventEndDate").val("" + end.format("MM/dd/yyyy"));

    addStartDate = start;
    addEndDate = end;
    globalAllDay = allDay;
    //alert(allDay);

}

function updateEventOnDropResize(event, allDay) {
    debugger;
    //alert("allday: " + allDay);
    var eventToUpdate = {
        id: event.id,
        start: event.start

    };

    if (allDay) {
        eventToUpdate.start.setHours(0, 0, 0);

    }

    if (event.end === null) {
        eventToUpdate.end = eventToUpdate.start;

    }
    else {
        eventToUpdate.end = event.end;
        if (allDay) {
            eventToUpdate.end.setHours(0, 0, 0);
        }
    }

    eventToUpdate.start = eventToUpdate.start.format("dd-MM-yyyy hh:mm:ss tt");
    eventToUpdate.end = eventToUpdate.end.format("dd-MM-yyyy hh:mm:ss tt");

    PageMethods.UpdateEventTime(eventToUpdate, UpdateTimeSuccess);

}

function eventDropped(event, dayDelta, minuteDelta, allDay, revertFunc) {
    debugger;
    if ($(this).data("qtip")) $(this).qtip("destroy");

    updateEventOnDropResize(event, allDay);



}

function eventResized(event, dayDelta, minuteDelta, revertFunc) {
    debugger;
    if ($(this).data("qtip")) $(this).qtip("destroy");

    updateEventOnDropResize(event);

}

function checkForSpecialChars(stringToCheck) {
    //var pattern = /[^A-Za-z0-9 ]/;
    //return pattern.test(stringToCheck);
    return false;
}

function delEvent() {
    if (confirm("do you really want to delete this event?")) {

        PageMethods.deleteEvent($("#eventId").val(), deleteSuccess);


        debugger;
        $('#calendar').fullCalendar('removeEvents', $("#eventId").val());

        $('#addDialog').dialog("close");
    }
}

$(document).ready(function () {

    $('#fileUploadPhotAdds').click(function (event) {
        event.preventDefault();
        FileUp();
    });

    //$('#fileUploadPhotosAdd').change(function (event) {       
    //    $('#spnPhoto').text($("#fileUploadPhotosAdd").val());
    //});

    $('#fuAttach').click(function (event) {
        event.preventDefault();
        FileUpAttach();
    });

    //$('#fileUploadAttachmentsAdd').change(function (event) {

    //    $('#spnAttach').text($("#fileUploadAttachmentsAdd").val());
    //});

    // update Dialog
    $('#updatedialog').dialog({
        autoOpen: false,
        width: 470,
        buttons: {
            "update": function () {
                debugger;
                //alert(currentUpdateEvent.title);

                Update();
                return;

                var eventToUpdate = {
                    id: currentUpdateEvent.id,
                    //nameOfVisitor: $("#txtNameOfVisitor").val(),

                    //designtionOfVisitor: $("#txtDesignOfVisitor").val(),
                    //nameOfVisitorOrganisation: $("#txtNameOfVisitorOrg").val(),
                    //appointment: $("#txtAppointment").val(),
                    //nameDesignation: $("#txtNameDesignation").val(),
                    //uploadPhoto: $("#fileUploadPhoto").val(),
                    //attachments: $("#fileUploadAttachments").val(),
                    //information: $("#txtInformation").val(),

                    title: $("#txtNameOfVisitorUpdate").val(),
                    nameOfVisitor: $("#txtNameOfVisitorUpdate").val(),

                    designtionOfVisitor: $("#txtDesignOfVisitorUpdate").val(),
                    nameOfVisitorOrganisation: $("#txtNameOfVisitorOrgUpdate").val(),
                    appointment: $("#txtAppointmentUpdate").val(),
                    nameDesignation: $("#txtNameDesignationUpdate").val(),
                    uploadPhoto: $("#fileUploadPhotoUpdate").val(),
                    attachments: $("#fileUploadAttachmentsUpdate").val(),
                    information: $("#txtInformationUpdate").val()
                };



                if (checkForSpecialChars(eventToUpdate.nameOfVisitor) || checkForSpecialChars(eventToUpdate.designtionOfVisitor) || checkForSpecialChars(eventToUpdate.nameOfVisitorOrganisation) || checkForSpecialChars(eventToUpdate.appointment) || checkForSpecialChars(eventToUpdate.nameDesignation) || checkForSpecialChars(eventToUpdate.uploadPhoto) || checkForSpecialChars(eventToUpdate.attachments) || checkForSpecialChars(eventToUpdate.information)) {
                    alert("please enter characters: A to Z, a to z, 0 to 9, spaces");
                }
                else {
                    PageMethods.UpdateEvent(eventToUpdate, updateSuccess);


                    currentUpdateEvent.nameOfVisitor = $("#txtNameOfVisitorUpdate").val();
                    currentUpdateEvent.designtionOfVisitor = $("#txtDesignOfVisitorUpdate").val();
                    currentUpdateEvent.nameOfVisitorOrganisation = $("#txtNameOfVisitorOrgUpdate").val();
                    currentUpdateEvent.appointment = $("#txtAppointmentUpdate").val();
                    currentUpdateEvent.nameDesignation = $("#txtNameDesignationUpdate").val();
                    currentUpdateEvent.uploadPhoto = $("#fileUploadPhotoUpdate").val();
                    currentUpdateEvent.attachments = $("#fileUploadAttachmentsUpdate").val();
                    currentUpdateEvent.information = $("#txtInformationUpdate").val();
                    currentUpdateEvent.title = $("#txtNameOfVisitorUpdate").val();
                    $('#calendar').fullCalendar('updateEvent', currentUpdateEvent);
                    // $(this).dialog("close");
                }
            }
            //,
            //"delete": function () {

            //    if (confirm("do you really want to delete this event?")) {

            //        PageMethods.deleteEvent($("#eventId").val(), deleteSuccess);
            //        alert($("#eventId").val());

            //        debugger;
            //        $('#calendar').fullCalendar('removeEvents', $("#eventId").val());

            //        $(this).dialog("close");
            //    }

            //}

        }//,
        //    close: function (event, ui) {
        //        $("#txtNameOfVisitorUpdate").val("");
        //        $("#txtDesignOfVisitorUpdate").val("");
        //        $("#txtNameOfVisitorOrgUpdate").val("");
        //        $("#txtAppointmentUpdate").val("");
        //        $("#txtNameDesignationUpdate").val("");
        //        $("#fileUploadPhotoUpdate").val("");
        //        $("#fileUploadAttachmentsUpdate").val("");
        //        $("#txtInformationUpdate").val("");
        //        $("#eventId").val("");
        //        $("#eventStartUpdate").text("");

        //}
    }).parent().appendTo("form:first");

    //add dialog
    $('#addDialog').dialog({
        autoOpen: false,
        width: 470,
        buttons: {
            "Add": function () {
                Add();
                return;
                //alert("sent:" + addStartDate.format("dd-MM-yyyy hh:mm:ss tt") + "==" + addStartDate.toLocaleString());
                var eventToAdd = {
                    nameOfVisitor: $("#txtAddNameOfVisitor").val(),
                    designtionOfVisitor: $("#txtAddDesignOfVisitor").val(),
                    nameOfVisitorOrganisation: $("#txtAddNameOfVisitorOrg").val(),
                    appointment: $("#txtAddAppointment").val(),
                    nameDesignation: $("#txtAddNameDesignation").val(),
                    //uploadPhoto: $("#fileUploadPhotoAdd").val(),
                    //attachments: $("#fileUploadAttachmentsAdd").val(),
                    information: $("#AddtxtInformation").val(),
                    start: addStartDate.format("dd-MM-yyyy"),
                    end: addEndDate.format("dd-MM-yyyy")

                };

                if (checkForSpecialChars(eventToAdd.nameOfVisitor) || checkForSpecialChars(eventToAdd.designtionOfVisitor) || checkForSpecialChars(eventToAdd.nameOfVisitorOrganisation) || checkForSpecialChars(eventToAdd.appointment) || checkForSpecialChars(eventToAdd.nameDesignation) || checkForSpecialChars(eventToAdd.information)) {
                    alert("please enter characters: A to Z, a to z, 0 to 9, spaces");
                }
                else {
                    //alert("sending " + eventToAdd.title);
                    debugger;
                    PageMethods.addEvent(eventToAdd, addSuccess);
                    $(this).dialog("close");


                }

            }

        }
        //,
        //close: function (event, ui) {
        //    $("#txtAddNameOfVisitor").val("");
        //    $("#txtAddNameOfVisitor").val("");
        //    $("#txtAddDesignOfVisitor").val("");
        //    $("#txtAddNameOfVisitorOrg").val("");
        //   $("#txtAddAppointment").val("");
        //    $("#txtAddNameDesignation").val("");
        //    //uploadPhoto: "test",//$("#fileUploadPhotoAdd").val(),
        //    //attachments: "ters",//$("#fileUploadAttachmentsAdd").val(),
        //    $("#AddtxtInformation").val("");

        //}
    }).parent().appendTo("form");

    debugger;
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    var calendar = $('#calendar').fullCalendar({
        theme: true,
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        eventClick: updateEvent,
        selectable: true,
        selectHelper: true,
        select: selectDate,
        editable: true,
        events: "JsonResponse.ashx",
        eventDrop: eventDropped,
        eventResize: eventResized,
        eventRender: function (event, element) {
            //alert(event.title);
            element.qtip({
                content: event.information,
                position: { corner: { tooltip: 'bottomLeft', target: 'topRight' } },
                style: {
                    border: {
                        width: 1,
                        radius: 3,
                        color: '#2779AA'

                    },
                    padding: 10,
                    textAlign: 'center',
                    tip: true, // Give it a speech bubble tip with automatic corner detection
                    name: 'cream' // Style it according to the preset 'cream' style
                }

            });
        }

    });   
});