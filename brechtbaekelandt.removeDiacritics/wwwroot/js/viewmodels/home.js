var brechtbaekelandt = brechtbaekelandt || {};

brechtbaekelandt.removeDiacritics = (function ($, jQuery, ko, undefined) {
    "use strict";

    function HomeViewModel() {
        const self = this;

        self.filter = ko.observable();
        self.filter.subscribe((filter) => {
            if (filter) {
                self.filterWords(filter);
            } else {
                self.getAllWords();
            }
        });

        self.words = ko.observableArray();

        self.getAllWords();
    };

    HomeViewModel.prototype.getAllWords = function () {
        const self = this;

        $.ajax({
            url: "../api/data",
            contentType: "application/json",
            type: "get"
        })
            .done(function (data, textStatus, jqXhr) {
                self.words(data);
            })
            .fail(function (jqXhr, textStatus, errorThrown) {

            });
    }

    HomeViewModel.prototype.filterWords = function (filter) {
        const self = this;
        $.ajax({
            url: `../api/data?filter=${filter}`,
            contentType: "application/json",
            type: "get"
        })
            .done(function (data, textStatus, jqXhr) {
                self.words(data);
            })
            .fail(function (jqXhr, textStatus, errorThrown) {

            });
    }

    function init() {
        const viewModel = new HomeViewModel();

        ko.applyBindings(viewModel);
    };

    return {
        HomeViewModel: HomeViewModel,
        init: init
    };

})($, jQuery, ko);