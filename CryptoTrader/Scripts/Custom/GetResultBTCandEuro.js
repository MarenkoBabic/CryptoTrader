$("#BTC").keyup(function () {
    $.ajax({
        url: "Trade/GetEuro",
        type: 'POST',
        data: {
            BTCAmount: $("#BTC").val()
        },
        success: function (data) {
            $("#Euro").val(data);
        }
    });
});

$("#Euro").keyup(function () {
    $.ajax({
        url: "Trade/GetBTC",
        type: 'POST',
        data: {
            EuroAmount: $("#Euro").val()
        },
        success: function (data) {
            $("#BTC").val(data);
        }
    });
});
