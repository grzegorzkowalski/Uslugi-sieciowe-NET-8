"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7128/postcardHub").build();

document.getElementById("send").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${message}`;
});

connection.on("ReceiveCard", function (link) {
    var li = document.createElement("li");
    var newA = document.createElement("a");
    newA.href = link;
    newA.textContent = "Twoja pocztówka";
    li.append(newA);
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("send").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("send").addEventListener("click", function (event) {
    var user = document.getElementById("user").value;
    var mail = document.getElementById("mail").value;
    var prompt = document.getElementById("prompt").value;
    connection.invoke("SendMail", user, mail, prompt).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});