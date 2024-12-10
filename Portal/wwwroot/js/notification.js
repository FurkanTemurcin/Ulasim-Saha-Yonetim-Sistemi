if (typeof signalR !== "undefined") {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    connection.start()
        .then(() => console.log("SignalR bağlantısı başarıyla kuruldu"))
        .catch(err => console.error("SignalR bağlantı hatası:", err));

    connection.on("ReceiveNotification", function (message) {
        alert("Yeni Bildirim: " + message);
    });
} else {
    console.error("SignalR kütüphanesi yüklenmedi.");
}
