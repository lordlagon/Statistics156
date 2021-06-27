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
function GenerateLineChart(consultaPorTipo) {
    am4core.ready(function () {
        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create("chartdiv", am4charts.XYChart);

        chart.data = consultaPorTipo;

        // Create axes
        var dateAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        dateAxis.dataFields.category = "ano"

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

        //// Create series
        var series = chart.series.push(new am4charts.ColumnSeries());
        series.dataFields.valueY = "count";
        series.dataFields.categoryX = "ano";

        series.columns.template.tooltipText = "Series: {tipo}\nCategory: {categoryX}\nValue: {valueY}";
        //series.tooltip.pointerOrientation = "vertical";

        //chart.cursor = new am4charts.XYCursor();
        //chart.cursor.snapToSeries = series;
        //chart.cursor.xAxis = dateAxis;

        //chart.scrollbarY = new am4core.Scrollbar();
        chart.scrollbarX = new am4core.Scrollbar();

    }); // end am4core.ready()
}