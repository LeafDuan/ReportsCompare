$(function () {
    registerAuto();
    $("form").validate();
    $("form").submit(function () {
        var form = $(this);
        if (!form.valid()) {
            return;
        }
        getFiles(form.serialize());
    });
});

function getFiles(query) {
    waitingDialog.show("Comparing ......");
    var url = "/api/file";
    $.getJSON(url,
        query,
        function (data) {
            //$.each(data, function (index) {
            //    report(data[index]);
            //});
            if (data.length === 0) {
                $(".reports").html(alert());
            } else {
                $(".reports").html("");
                report(data);
            }
        });
}

function report(files) {
    //console.log(files.length);
    if (files.length === 0) {
        waitingDialog.hide();
        return;
    }

    var file = files[0];
    var url = "/api/report";
    $.getJSON(url,
        { file: file },
        function(data) {
            $(".reports").append(template(file, data));
            report(files.slice(1));
        });
}

function template(file, data) {
    var header = data[0];
    var tpl = [];
    tpl.push("<h5>" + file + "</h5>");
    tpl.push("<table class='table table-striped table-bordered'>");
    tpl.push("<thead>");
    tpl.push("<tr>");
    $.each(header,
        function(index) {
            tpl.push("<th>" + header[index] + "</th>");
        });
    tpl.push("</tr>");
    tpl.push("</thead>");
    tpl.push("<tbody>");
    data = data.slice(1);
    $.each(data,
        function(index) {
            tpl.push("<tr>");
            var values = data[index];
            $.each(values,
                function(col) {
                    tpl.push("<td>" + (col !== 0 ? formatPrice(values[col]) : values[col]) + "</td>");
                });
            tpl.push("</tr>");
        });
    tpl.push("</tbody>");
    tpl.push("</table>");

    return tpl.join("\n");
}

function formatPrice(value) {
    var temp = value;
    var price = parseFloat(temp) || 0;
    if (price.toString().length === temp.length && (price > 999 || -999 > price)) {
        temp = new Intl.NumberFormat().format(price);
        if ((9999999 < price && price < 100000000) || (-9999999 > price && price > -100000000)) {
            temp += ("</br>" + (price / 10000000).toFixed(2) + " 千万");
        } else if ((99999999 < price) || (-99999999 > price)) {
            temp += ("</br>" + (price / 100000000).toFixed(2) + " 亿");
        }
    }

    return temp;
}

function alert() {
    var tpl = [];
    tpl.push('<div class="alert alert-warning alert-dismissible fade in" role="alert">');
    tpl.push('<button type="button" class="close" data-dismiss="alert" aria-label="Close">');
    tpl.push('<span aria-hidden="true">&times;</span>');
    tpl.push("</button >");
    tpl.push("<strong>Oh snap!</strong> There has no data.");
    tpl.push("</div>");

    return tpl.join("\n");
}

function registerAuto() {
    $(".auto-stock")
        .autocomplete({
            source: function(request, response) {
                var term = $.trim(request.term);
                if (term.length === 0) {
                    return;
                }
                $.getJSON("/api/query",
                    { keyword: term },
                    function(data) {
                        response(parseStock($.parseJSON(data)));
                    });
            },
            delay: 100,
            autoFocus: true,
            minLength: 2,
            position: { my: "left top", at: "left buttom" },
            select: function(event, ui) {
                //console.log(ui.item);
                setQuery(ui.item.label);
            },
            change: function(event, ui) {
                //console.log(ui);
                if (!ui.item) {
                    $(this).val("");
                }
            }
        })
        .data("ui-autocomplete")
        ._renderItem = function(ul, item) {
            return $("<li>")
                .attr("data-value", item.value)
                .append("<div><dd>" + item.label.code + "</dd><dd>" + item.label.category + "</dd><dd>" + item.label.zwjc + "</dd></div>")
                .appendTo(ul);
        }
}

function parseStock(stocks) {
    /**
     {
    "startTime": "1994",
    "orgId": "gssz0000565",
    "category": "A股",
    "market": "sz",
    "code": "000565",
    "pinyin": "ysxa",
    "zwjc": "渝三峡Ａ"
    },
     */
    var data = [];
    $.each(stocks,
        function(index) {
            var stock = stocks[index];
            data.push({ label: stock, value: stock.code });
        });

    return data;
}

function setQuery(stock) {
    var minYear = $("#minYear");
    var maxYear = $("#maxYear");
    var market = $("#market");
    var orgId = $("#orgId");

    market.val(stock.market);
    orgId.val(stock.orgId);
    minYear.html("");
    maxYear.html("");
    var endTime = new Date().getFullYear();
    var startTime = parseInt(stock.startTime) || endTime;
    for (var i = startTime; i <= endTime; i++) {
        var selected = (i === endTime ? "selected='selected'" : "");
        minYear.append("<option value='" + i + "' " + selected + ">" + i + "</option>");
        maxYear.append("<option value='" + i + "' " + selected + ">" + i + "</option>");
    }
}