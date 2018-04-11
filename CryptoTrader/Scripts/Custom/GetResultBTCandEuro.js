$("#BTC").keyup(function () {
    $.ajax({
        url: "Trade/GetEuro",
        type: 'POST',
        data: {
            BtcTrade: $("#BTC").val().replace(".", ",")
        },
        success: function (data) {
            vm: {
                EuroTrade: $('#Euro').val(data - '0.1')
            }
        }
    });
});

$("#Euro").keyup(function () {
    $.ajax({
        url: "Trade/GetBTC",
        type: 'POST',
        data: {
            EuroTrade: $("#Euro").val().replace(".", ",")
        },
        success: function (data) {
            BtcTrade: $("#BTC").val(data - '0.1')
        }
    });
});
