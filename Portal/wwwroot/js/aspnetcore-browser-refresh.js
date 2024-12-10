(function () {
    if (typeof WebSocket === 'function') {
        var url = 'wss://' + window.location.host + '/_framework/aspnetcore-browser-refresh';
        var socket = new WebSocket(url);

        socket.onmessage = function (message) {
            if (message.data === 'Reload') {
                window.location.reload();
            }
        };

        socket.onclose = function () {
            console.log('Browser refresh socket closed.');
        };
    }
})();
