var brechtbaekelandt = brechtbaekelandt || {};

brechtbaekelandt.enableODataQueryOnComputedProperties = (function ($, jQuery, ko, undefined) {
    "use strict";

    function HomeViewModel() {
        const self = this;

        self.persons = ko.observableArray();
        self.personsJsonString = ko.observable();
        self.simplePersons = ko.observableArray();
        self.simplePersonsJsonString = ko.observable();
        self.orderedByNamePersons = ko.observableArray();
        self.orderedByNamePersonsJsonString = ko.observable();
        self.orderedByAgePersons = ko.observableArray();
        self.orderedByAgePersonsJsonString = ko.observable();

        self.getPersons();
        self.getSimplePersons();
        self.getOrderedByNamePersons();
        self.getOrderedByAgePersons();
    };

    HomeViewModel.prototype.getPersons = function () {
        const self = this;

        $.ajax({
            url: "../odata/Persons",
            contentType: "application/json",
            dataType: "json",
            type: "get"
        })
            .done(function (data, textStatus, jqXhr) {
                self.persons(data.value);
                self.personsJsonString(JSON.stringify(data.value));
            })
            .fail(function (jqXhr, textStatus, errorThrown) {

            });
    }

    HomeViewModel.prototype.getSimplePersons = function () {
        const self = this;

        $.ajax({
                url: "../odata/Persons?$select=Firstname,Name",
                contentType: "application/json",
                dataType: "json",
                type: "get"
            })
            .done(function (data, textStatus, jqXhr) {
                self.simplePersons(data.value);
                self.simplePersonsJsonString(JSON.stringify(data.value));
            })
            .fail(function (jqXhr, textStatus, errorThrown) {

            });
    }

    HomeViewModel.prototype.getOrderedByNamePersons = function () {
        const self = this;

        $.ajax({
                url: "../odata/Persons?$orderby=Name desc",
                contentType: "application/json",
                dataType: "json",
                type: "get"
            })
            .done(function (data, textStatus, jqXhr) {
                self.orderedByNamePersons(data.value);
                self.orderedByNamePersonsJsonString(JSON.stringify(data.value));
            })
            .fail(function (jqXhr, textStatus, errorThrown) {

            });
    }

    HomeViewModel.prototype.getOrderedByAgePersons = function () {
        const self = this;

        $.ajax({
            url: "../odata/Persons?$select=Lastname,Birthday,Age&$orderby=Age",
                contentType: "application/json",
                dataType: "json",
                type: "get"
            })
            .done(function (data, textStatus, jqXhr) {
                self.orderedByAgePersons(data.value);
                self.orderedByAgePersonsJsonString(JSON.stringify(data.value));
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