$(document).ready(function () {
    UpdateChart();
});

function UpdateChart() {
    if ($("collapseExample").hasClass("in") == false) {
        $.ajax({
            url: '/Ticker/LoadChartData',
            success: function (json) {
                CreateChart(json);
            },
            error: function (errorData) {
                console.log(errorData);
            }
        });
        setTimeout(function () { UpdateChart(); }, 5000);
    }
}

function CreateChart(jsonData) {
    Highcharts.stockChart('Chart', {
        title: {
            text: 'Aktueller BitCoin Kurs'
        },
        rangeSelector: false,
        series: [{
            name: 'BTC/EUR',
            data: jsonData,
            //type: 'area',
            tooltip: {
                valueDecimals: 2
            }
        }],
    });
}
