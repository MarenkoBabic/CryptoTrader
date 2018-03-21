$("#BTC").keyup(function () {
    $.ajax({
        url: "Trade/GetEuro",
        type: 'POST',
        data: {
            BtcTrade: $("#BTC").val().replace(".", ",")
        },
        success: function (data) {
            vm: {
                EuroTrade: $('#Euro').val(data)
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
            BtcTrade: $("#BTC").val(data)
        }
    });
});
//function SubmitCointrade(sellOrBuy) {
//    $.ajax({
//        url: "Trade/Trade",
//        type: 'POST',
//        data: {
//            vm: {
//                EuroTrade: $("#Euro").val().replace(".", ","),
//                BTCTrade: $("#BTC").val().replace(".", ",")
//            },
//            submit: sellOrBuy
//        },
//        success: function (data) {

//        }
//    });
//}