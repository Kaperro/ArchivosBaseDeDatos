"use strict";

function retrieveData() {
    var model = $('#data_table');
    $.ajax({
        url: '/Documentos/RetrieveDataAsync',
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html',
        success: (function (result) { model.empty().append(result); }),
    })
}

$(document).ready(retrieveData);

var connection = new signalR.HubConnectionBuilder().withUrl("/mainHub").build();

connection.on("CheckGroupTray", function () {
    retrieveData()
});

connection.start().catch(function (e) {
    return console.error(e.toString());
});