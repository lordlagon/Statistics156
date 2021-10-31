function GeneratePieChart(chartdiv, assuntos) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create(chartdiv, am4charts.PieChart);

        // Add data

        chart.data = assuntos;

        // Add and configure Series
        var pieSeries = chart.series.push(new am4charts.PieSeries());
        pieSeries.dataFields.value = "count";
        pieSeries.dataFields.category = "assunto";
        pieSeries.slices.template.stroke = am4core.color("#fff");
        pieSeries.slices.template.strokeOpacity = 1;
        pieSeries.slices.template.tooltipText = "Assunto: {category} Quantidade: {value}";
        // This creates initial animation
        pieSeries.hiddenState.properties.opacity = 1;
        pieSeries.hiddenState.properties.endAngle = -90;
        pieSeries.hiddenState.properties.startAngle = -90;

        chart.hiddenState.properties.radius = am4core.percent(0);


    }); // end am4core.ready()
}
function GenerateLineChart(assuntoPorBairro) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdivline", am4charts.XYChart);

        // Add data
        //chart.data = [{
        //    "date": "2012-07-27",
        //    "value": 13
        //}, {
        chart.data = assuntoPorBairro;

        // Set input format for the dates
        chart.dateFormatter.inputDateFormat = "yyyy-MM-dd";

        // Create axes
        var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

        // Create series
        var series = chart.series.push(new am4charts.LineSeries());
        series.dataFields.valueY = "count";
        series.dataFields.dateX = "datacompleta";
        series.tooltipText = "{count}"
        series.strokeWidth = 2;
        series.minBulletDistance = 15;

        // Drop-shaped tooltips
        series.tooltip.background.cornerRadius = 20;
        series.tooltip.background.strokeOpacity = 0;
        series.tooltip.pointerOrientation = "vertical";
        series.tooltip.label.minWidth = 40;
        series.tooltip.label.minHeight = 40;
        series.tooltip.label.textAlign = "middle";
        series.tooltip.label.textValign = "middle";

        // Make bullets grow on hover
        var bullet = series.bullets.push(new am4charts.CircleBullet());
        bullet.circle.strokeWidth = 2;
        bullet.circle.radius = 4;
        bullet.circle.fill = am4core.color("#fff");

        var bullethover = bullet.states.create("hover");
        bullethover.properties.scale = 1.3;

        // Make a panning cursor
        chart.cursor = new am4charts.XYCursor();
        chart.cursor.behavior = "panXY";
        chart.cursor.xAxis = dateAxis;
        chart.cursor.snapToSeries = series;

        // Create vertical scrollbar and place it before the value axis
        chart.scrollbarY = new am4core.Scrollbar();
        chart.scrollbarY.parent = chart.leftAxesContainer;
        chart.scrollbarY.toBack();

        // Create a horizontal scrollbar with previe and place it underneath the date axis
        chart.scrollbarX = new am4charts.XYChartScrollbar();
        chart.scrollbarX.series.push(series);
        chart.scrollbarX.parent = chart.bottomAxesContainer;

        dateAxis.start = 0.79;
        dateAxis.keepSelection = true;
    });
}
function GenerateFourColumnChart(assuntoPorBairro) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create('chartdivline', am4charts.XYChart)
        chart.colors.step = 2;

        chart.legend = new am4charts.Legend()
        chart.legend.position = 'top'
        chart.legend.paddingBottom = 20
        chart.legend.labels.template.maxWidth = 95

        var xAxis = chart.xAxes.push(new am4charts.CategoryAxis())
        xAxis.dataFields.category = 'category'
        xAxis.renderer.cellStartLocation = 0.1
        xAxis.renderer.cellEndLocation = 0.9
        xAxis.renderer.grid.template.location = 0;

        var yAxis = chart.yAxes.push(new am4charts.ValueAxis());
        yAxis.min = 0;

        function createSeries(value, name) {
            var series = chart.series.push(new am4charts.ColumnSeries())
            series.dataFields.valueY = value
            series.dataFields.categoryX = 'category'
            series.name = name

            series.events.on("hidden", arrangeColumns);
            series.events.on("shown", arrangeColumns);

            var bullet = series.bullets.push(new am4charts.LabelBullet())
            bullet.interactionsEnabled = false
            bullet.dy = 30;
            bullet.label.text = '{valueY}'
            bullet.label.fill = am4core.color('#ffffff')

            return series;
        }

        chart.data = [
            {
                category: 'Junho',
                first: 40,
                second: 55,
                third: 60
            },
            {
                category: 'Julho',
                first: 30,
                second: 78,
                third: 69
            },
            {
                category: 'Agosto',
                first: 27,
                second: 40,
                third: 45
            },
            {
                category: 'Setembro',
                first: 50,
                second: 33,
                third: 22
            }
        ]

        createSeries('first', 'Ahu');
        createSeries('second', 'Boa Vista');
        createSeries('third', 'Centro');

        function arrangeColumns() {

            var series = chart.series.getIndex(0);

            var w = 1 - xAxis.renderer.cellStartLocation - (1 - xAxis.renderer.cellEndLocation);
            if (series.dataItems.length > 1) {
                var x0 = xAxis.getX(series.dataItems.getIndex(0), "categoryX");
                var x1 = xAxis.getX(series.dataItems.getIndex(1), "categoryX");
                var delta = ((x1 - x0) / chart.series.length) * w;
                if (am4core.isNumber(delta)) {
                    var middle = chart.series.length / 2;

                    var newIndex = 0;
                    chart.series.each(function (series) {
                        if (!series.isHidden && !series.isHiding) {
                            series.dummyData = newIndex;
                            newIndex++;
                        }
                        else {
                            series.dummyData = chart.series.indexOf(series);
                        }
                    })
                    var visibleCount = newIndex;
                    var newMiddle = visibleCount / 2;

                    chart.series.each(function (series) {
                        var trueIndex = chart.series.indexOf(series);
                        var newIndex = series.dummyData;

                        var dx = (newIndex - trueIndex + middle - newMiddle) * delta

                        series.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                        series.bulletsContainer.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                    })
                }
            }
        }

    }); // end am4core.ready()
}
function GenerateColumnChart(bairroPorAssunto) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdiv", am4charts.XYChart);

        chart.data = bairroPorAssunto;

        var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "bairro";
        categoryAxis.renderer.grid.template.location = 0;
        categoryAxis.renderer.minGridDistance = 150;

        categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
            if (target.dataItem && target.dataItem.index & 2 == 2) {
                return dy + 25;
            }
            return dy;
        });

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries());
        series.dataFields.valueY = "count";
        series.dataFields.categoryX = "bairro";
        series.name = "Count";
        series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
        series.columns.template.fillOpacity = .8;

        var columnTemplate = series.columns.template;
        columnTemplate.strokeWidth = 2;
        columnTemplate.strokeOpacity = 1;

    }); // end am4core.ready()
}
function GenerateFaixaEtariaChart(faixas_etarias) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var mainContainer = am4core.create("chartdivFaixa", am4core.Container);
        mainContainer.width = am4core.percent(100);
        mainContainer.height = am4core.percent(100);
        mainContainer.layout = "horizontal";

        var usData = [
            {
                "faixa_etaria": "0 a 4",
                "male": 10175713,
                "female": 9736305
            },
            {
                "faixa_etaria": "5 to 9",
                "male": 10470147,
                "female": 10031835
            },
            {
                "faixa_etaria": "10 to 14",
                "male": 10561873,
                "female": 10117913
            },
            {
                "faixa_etaria": "15 to 19",
                "male": 6447043,
                "female": 6142996
            },
            {
                "faixa_etaria": "20 a 24",
                "male": 9349745,
                "female": 8874664
            },            
            {
                "faixa_etaria": "25 a 29",
                "male": 10989596,
                "female": 10708414
            },
            {
                "faixa_etaria": "30 a 34",
                "male": 10625791,
                "female": 10557848
            },
            {
                "faixa_etaria": "35 a 39",
                "male": 9899569,
                "female": 9956213
            },
            {
                "faixa_etaria": "40 a 44",
                "male": 10330986,
                "female": 10465142
            },
            {
                "faixa_etaria": "45 a 49",
                "male": 10571984,
                "female": 10798384
            },
            {
                "faixa_etaria": "50 a 54",
                "male": 11051409,
                "female": 11474081
            },
            {
                "faixa_etaria": "55 a 59",
                "male": 10173646,
                "female": 10828301
            },
            {
                "faixa_etaria": "60 a 64",
                "male": 8824852,
                "female": 9590829
            },
            {
                "faixa_etaria": "65 a 69",
                "male": 6876271,
                "female": 7671175
            },
            {
                "faixa_etaria": "70 a 74",
                "male": 4867513,
                "female": 5720208
            },
            {
                "faixa_etaria": "75 a 79",
                "male": 3416432,
                "female": 4313697
            },
            {
                "faixa_etaria": "80 a 84",
                "male": 2378691,
                "female": 3432738
            },
            {
                "faixa_etaria": "85 a 90",
                "male": 2000771,
                "female": 3937981
            },
            {
                "faixa_etaria": "90 ou mais",
                "male": 222438,
                "female": 634227
            }
        ];

        var maleChart = mainContainer.createChild(am4charts.XYChart);
        maleChart.paddingRight = 0;
        maleChart.data = JSON.parse(JSON.stringify(usData));

        // Create axes
        var maleCategoryAxis = maleChart.yAxes.push(new am4charts.CategoryAxis());
        maleCategoryAxis.dataFields.category = "faixa_etaria";
        maleCategoryAxis.renderer.grid.template.location = 0;
        //maleCategoryAxis.renderer.inversed = true;
        maleCategoryAxis.renderer.minGridDistance = 15;

        var maleValueAxis = maleChart.xAxes.push(new am4charts.ValueAxis());
        maleValueAxis.renderer.inversed = true;
        maleValueAxis.min = 0;
        maleValueAxis.max = 10;
        maleValueAxis.strictMinMax = true;

        maleValueAxis.numberFormatter = new am4core.NumberFormatter();
        maleValueAxis.numberFormatter.numberFormat = "#.#'%'";

        // Create series
        var maleSeries = maleChart.series.push(new am4charts.ColumnSeries());
        maleSeries.dataFields.valueX = "male";
        maleSeries.dataFields.valueXShow = "percent";
        maleSeries.calculatePercent = true;
        maleSeries.dataFields.categoryY = "faixa_etaria";
        maleSeries.interpolationDuration = 1000;
        maleSeries.columns.template.tooltipText = "Masculino, Faixa Etária{categoryY}: {valueX} ({valueX.percent.formatNumber('#.0')}%)";
        //maleSeries.sequencedInterpolation = true;


        var femaleChart = mainContainer.createChild(am4charts.XYChart);
        femaleChart.paddingLeft = 0;
        femaleChart.data = JSON.parse(JSON.stringify(usData));

        // Create axes
        var femaleCategoryAxis = femaleChart.yAxes.push(new am4charts.CategoryAxis());
        femaleCategoryAxis.renderer.opposite = true;
        femaleCategoryAxis.dataFields.category = "faixa_etaria";
        femaleCategoryAxis.renderer.grid.template.location = 0;
        femaleCategoryAxis.renderer.minGridDistance = 15;

        var femaleValueAxis = femaleChart.xAxes.push(new am4charts.ValueAxis());
        femaleValueAxis.min = 0;
        femaleValueAxis.max = 10;
        femaleValueAxis.strictMinMax = true;
        femaleValueAxis.numberFormatter = new am4core.NumberFormatter();
        femaleValueAxis.numberFormatter.numberFormat = "#.#'%'";
        femaleValueAxis.renderer.minLabelPosition = 0.01;

        // Create series
        var femaleSeries = femaleChart.series.push(new am4charts.ColumnSeries());
        femaleSeries.dataFields.valueX = "female";
        femaleSeries.dataFields.valueXShow = "percent";
        femaleSeries.calculatePercent = true;
        femaleSeries.fill = femaleChart.colors.getIndex(4);
        femaleSeries.stroke = femaleSeries.fill;
        //femaleSeries.sequencedInterpolation = true;
        femaleSeries.columns.template.tooltipText = "Feminino, Faixa Etária {categoryY}: {valueX} ({valueX.percent.formatNumber('#.0')}%)";
        femaleSeries.dataFields.categoryY = "faixa_etaria";
        femaleSeries.interpolationDuration = 1000;


        
    }); // end am4core.ready()
}
function GenerateGroupChart() {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdivGroup", am4charts.XYChart);

        // Add data
        chart.data = [
            {
                "region": "Central",
                "state": "North Dakota",
                "sales": 920
            },
            {
                "region": "Central",
                "state": "South Dakota",
                "sales": 1317
            },
            {
                "region": "Central",
                "state": "Kansas",
                "sales": 2916
            },
            {
                "region": "Central",
                "state": "Iowa",
                "sales": 4577
            },
            {
                "region": "Central",
                "state": "Nebraska",
                "sales": 7464
            },
            {
                "region": "Central",
                "state": "Oklahoma",
                "sales": 19686
            },
            {
                "region": "Central",
                "state": "Missouri",
                "sales": 22207
            },
            {
                "region": "Central",
                "state": "Minnesota",
                "sales": 29865
            },
            {
                "region": "Central",
                "state": "Wisconsin",
                "sales": 32125
            },
            {
                "region": "Central",
                "state": "Indiana",
                "sales": 53549
            },
            {
                "region": "Central",
                "state": "Michigan",
                "sales": 76281
            },
            {
                "region": "Central",
                "state": "Illinois",
                "sales": 80162
            },
            {
                "region": "Central",
                "state": "Texas",
                "sales": 170187
            },
            {
                "region": "East",
                "state": "West Virginia",
                "sales": 1209
            },
            {
                "region": "East",
                "state": "Maine",
                "sales": 1270
            },
            {
                "region": "East",
                "state": "District of Columbia",
                "sales": 2866
            },
            {
                "region": "East",
                "state": "New Hampshire",
                "sales": 7294
            },
            {
                "region": "East",
                "state": "Vermont",
                "sales": 8929
            },
            {
                "region": "East",
                "state": "Connecticut",
                "sales": 13386
            },
            {
                "region": "East",
                "state": "Rhode Island",
                "sales": 22629
            },
            {
                "region": "East",
                "state": "Maryland",
                "sales": 23707
            },
            {
                "region": "East",
                "state": "Delaware",
                "sales": 27453
            },
            {
                "region": "East",
                "state": "Massachusetts",
                "sales": 28639
            },
            {
                "region": "East",
                "state": "New Jersey",
                "sales": 35763
            },
            {
                "region": "East",
                "state": "Ohio",
                "sales": 78253
            },
            {
                "region": "East",
                "state": "Pennsylvania",
                "sales": 116522
            },
            {
                "region": "East",
                "state": "New York",
                "sales": 310914
            },
            {
                "region": "South",
                "state": "South Carolina",
                "sales": 8483
            },
            {
                "region": "South",
                "state": "Louisiana",
                "sales": 9219
            },
            {
                "region": "South",
                "state": "Mississippi",
                "sales": 10772
            },
            {
                "region": "South",
                "state": "Arkansas",
                "sales": 11678
            },
            {
                "region": "South",
                "state": "Alabama",
                "sales": 19511
            },
            {
                "region": "South",
                "state": "Tennessee",
                "sales": 30662
            },
            {
                "region": "South",
                "state": "Kentucky",
                "sales": 36598
            },
            {
                "region": "South",
                "state": "Georgia",
                "sales": 49103
            },
            {
                "region": "South",
                "state": "North Carolina",
                "sales": 55604
            },
            {
                "region": "South",
                "state": "Virginia",
                "sales": 70641
            },
            {
                "region": "South",
                "state": "Florida",
                "sales": 89479
            },
            {
                "region": "West",
                "state": "Wyoming",
                "sales": 1603
            },
            {
                "region": "West",
                "state": "Idaho",
                "sales": 4380
            },
            {
                "region": "West",
                "state": "New Mexico",
                "sales": 4779
            },
            {
                "region": "West",
                "state": "Montana",
                "sales": 5589
            },
            {
                "region": "West",
                "state": "Utah",
                "sales": 11223
            },
            {
                "region": "West",
                "state": "Nevada",
                "sales": 16729
            },
            {
                "region": "West",
                "state": "Oregon",
                "sales": 17431
            },
            {
                "region": "West",
                "state": "Colorado",
                "sales": 32110
            },
            {
                "region": "West",
                "state": "Arizona",
                "sales": 35283
            },
            {
                "region": "West",
                "state": "Washington",
                "sales": 138656
            },
            {
                "region": "West",
                "state": "California",
                "sales": 457731
            }
        ];

        // Create axes
        var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
        yAxis.dataFields.category = "state";
        yAxis.renderer.grid.template.location = 0;
        yAxis.renderer.labels.template.fontSize = 10;
        yAxis.renderer.minGridDistance = 10;

        var xAxis = chart.xAxes.push(new am4charts.ValueAxis());

        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries());
        series.dataFields.valueX = "sales";
        series.dataFields.categoryY = "state";
        series.columns.template.tooltipText = "{categoryY}: [bold]{valueX}[/]";
        series.columns.template.strokeWidth = 0;
        series.columns.template.adapter.add("fill", function (fill, target) {
            if (target.dataItem) {
                switch (target.dataItem.dataContext.region) {
                    case "Central":
                        return chart.colors.getIndex(0);
                        break;
                    case "East":
                        return chart.colors.getIndex(1);
                        break;
                    case "South":
                        return chart.colors.getIndex(2);
                        break;
                    case "West":
                        return chart.colors.getIndex(3);
                        break;
                }
            }
            return fill;
        });

        var axisBreaks = {};
        var legendData = [];

        // Add ranges
        function addRange(label, start, end, color) {
            var range = yAxis.axisRanges.create();
            range.category = start;
            range.endCategory = end;
            range.label.text = label;
            range.label.disabled = false;
            range.label.fill = color;
            range.label.location = 0;
            range.label.dx = -130;
            range.label.dy = 12;
            range.label.fontWeight = "bold";
            range.label.fontSize = 12;
            range.label.horizontalCenter = "left"
            range.label.inside = true;

            range.grid.stroke = am4core.color("#396478");
            range.grid.strokeOpacity = 1;
            range.tick.length = 200;
            range.tick.disabled = false;
            range.tick.strokeOpacity = 0.6;
            range.tick.stroke = am4core.color("#396478");
            range.tick.location = 0;

            range.locations.category = 1;
            var axisBreak = yAxis.axisBreaks.create();
            axisBreak.startCategory = start;
            axisBreak.endCategory = end;
            axisBreak.breakSize = 1;
            axisBreak.fillShape.disabled = true;
            axisBreak.startLine.disabled = true;
            axisBreak.endLine.disabled = true;
            axisBreaks[label] = axisBreak;

            legendData.push({ name: label, fill: color });
        }

        addRange("Central", "Texas", "North Dakota", chart.colors.getIndex(0));
        addRange("East", "New York", "West Virginia", chart.colors.getIndex(1));
        addRange("South", "Florida", "South Carolina", chart.colors.getIndex(2));
        addRange("West", "California", "Wyoming", chart.colors.getIndex(3));

        chart.cursor = new am4charts.XYCursor();


        var legend = new am4charts.Legend();
        legend.position = "right";
        legend.scrollable = true;
        legend.valign = "top";
        legend.reverseOrder = true;

        chart.legend = legend;
        legend.data = legendData;

        legend.itemContainers.template.events.on("toggled", function (event) {
            var name = event.target.dataItem.dataContext.name;
            var axisBreak = axisBreaks[name];
            if (event.target.isActive) {
                axisBreak.animate({ property: "breakSize", to: 0 }, 1000, am4core.ease.cubicOut);
                yAxis.dataItems.each(function (dataItem) {
                    if (dataItem.dataContext.region == name) {
                        dataItem.hide(1000, 500);
                    }
                })
                series.dataItems.each(function (dataItem) {
                    if (dataItem.dataContext.region == name) {
                        dataItem.hide(1000, 0, 0, ["valueX"]);
                    }
                })
            }
            else {
                axisBreak.animate({ property: "breakSize", to: 1 }, 1000, am4core.ease.cubicOut);
                yAxis.dataItems.each(function (dataItem) {
                    if (dataItem.dataContext.region == name) {
                        dataItem.show(1000);
                    }
                })

                series.dataItems.each(function (dataItem) {
                    if (dataItem.dataContext.region == name) {
                        dataItem.show(1000, 0, ["valueX"]);
                    }
                })
            }
        })

    }); // end am4core.ready()
}


