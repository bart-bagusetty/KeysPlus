
function DrawDashBoard(data) {
    var propData = chartData.makeData(data.PropDashboardData);
    var propOptions = chartOptions.makeOptions(data.PropDashboardData)
    var propChart = KeysChart.drawDoughnut('property-chart', propData, propOptions);
    document.getElementById('prop-chart-legends').innerHTML = propChart.generateLegend();
}