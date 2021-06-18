function GeneratePieChart(countrInfos) {
    am4core.ready(function () {
        am4core.useTheme(am4themes_animated);
        var chart = am4core.create("chartdiv", am4charts.PieChart3D);
        chart.hiddenState.properties.opacity = 0; 
        chart.legend = new am4charts.Legend();
        chart.data = countrInfos;
        var series = chart.series.push(new am4charts.PieSeries3D());
        series.dataFields.value = "deathPercent";
        series.dataFields.category = "name";

    }); 
}