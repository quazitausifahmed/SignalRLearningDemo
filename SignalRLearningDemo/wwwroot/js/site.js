// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const connection = new signalR.HubConnectionBuilder()
    .withUrl('/learningHub')
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on('ReceiveMessage', (message) => {
    $('#signalr-message-panel').prepend($('<div />').text(message));
});

connection.on('ConnectionIDRecived', (ID) => {
    $('#connectionID').val(ID);
});

$('#btn-broadcast').click(() => {
    var message = $('#broadcast').val();
    connection.invoke('BrodcastMessage', message)
        .catch(err => alert(err));
});

$('#btn-other-message').click(() => {
    var message = $('#broadcast').val();
    connection.invoke('SendToOther', message)
        .catch(err => alert(err));
});

$('#btn-self-message').click(() => {
    var message = $('#broadcast').val();
    connection.invoke('SendToCaller', message)
        .catch(err => alert(err));
});

$('#btn-individual-message').click(() => {
    var connectioID = $('#user-connection-id').val();
    var message = $('#broadcast').val();
    connection.invoke('SendToIndividual', connectioID, message)
        .catch(err => alert(err));
});

$('#btn-broadcast-group').click(() => {
    var userGroup = $('#user-group').val();
    var message = $('#broadcast-group').val();
    connection.invoke('SendToGroup', userGroup, message)
        .catch(err => alert(err));
});

$('#btn-individual-message').click(() => {
    var connectioID = $('#user-connection-id').val();
    var message = $('#broadcast').val();
    connection.invoke('AddUserToGroup', connectioID, message)
        .catch(err => alert(err));
});

$('#btn-individual-message').click(() => {
    var connectioID = $('#user-connection-id').val();
    var message = $('#broadcast').val();
    connection.invoke('RemoveUserFromGroup', connectioID, message)
        .catch(err => alert(err));
});

async function start() {
    try {
        await connection.start();
        alert('connected');
        connection.invoke('GetConnectionID').catch(err => alert(err));
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
}

connection.onclose(async () => {
    await start();
});

start();
