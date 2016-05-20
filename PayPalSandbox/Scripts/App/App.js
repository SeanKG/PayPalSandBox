
(function () {



    var app = angular.module('exploreApp', []);


    function AppCtrl($http) {
        var ctrl = this,
            loading = {
                payments: true,
                cards: true
            },
            defaultNewPayment = {
                card_id: null,
                invoice_number: null,
                total: null,
                description: null,
                return_url: window.location.origin + window.location.pathname,
                cancel_url: window.location.origin + window.location.pathname
            },
            defaultNewCard = {
                expire_month: 11,
                expire_year: 2018,
                number: 4877274905927862,
                type: 'visa',
                cvv2: 874,
                external_customer_id: null,
            };

        ctrl.loading = loading;

        ctrl.getPaymentsList = function () {
            $http.get('./api/payments/list').then(function (result) {
                ctrl.paymentsList = result.data;
                ctrl.loading.payments = false;
            });
        }

        ctrl.getCardsList = function () {
            $http.get('./api/cards/list').then(function (result) {
                ctrl.cardsList = result.data;
                ctrl.loading.cards = false;
            });
        }

        ctrl.getPaymentStateClass = function (payment) {
            return {
                'panel-success': payment.state === 'approved',
                'panel-warning': payment.state === 'created'
        }
        }

        ctrl.getCardStateClass = function (card) {
            return {
                'panel-success': card.state === 'ok'
            }
        }

        ctrl.createPaypalPayment = function () {
            var url,
                isPaypal = false;
            console.log(ctrl.newPaypalPayment);
            if (ctrl.newPaypalPayment.invoice_number && ctrl.newPaypalPayment.total) {
                if (ctrl.newPaypalPayment.card_id) {
                    url = './api/payments/card';
                } else {
                    url = './api/payments/paypal';
                    isPaypal = true;
                }
                $http.post(url, ctrl.newPaypalPayment).then(function (result) {
                    ctrl.paymentsList.payments.unshift(result.data);
                    ctrl.newPaypalPayment = angular.copy(defaultNewPayment);
                    if (isPaypal) {

                    }
                },function(err) {
                    console.log(err);
                });
            }
        }

        ctrl.findRedirectLink = function (links) {
            var link;
            links.forEach(function(el, index, array) {
                if (el.rel === 'approval_url') {
                    link = el.href;
                }
            });
            return link;
        }

        ctrl.createCard = function () {
            console.log(ctrl.newCard);
            ctrl.loading.cards = true;
            if (true) {
                $http.post('./api/cards/create', ctrl.newCard).then(function (result) {
                    ctrl.cardsList.items.unshift(result.data);
                    ctrl.newCard = angular.copy(defaultNewPayment);
                    ctrl.loading.cards = false;
                }, function (err) {
                    ctrl.loading.cards = false;
                    console.log(err);
                });
            }
        }

        ctrl.deleteCard = function(card) {
            console.log(card);
            ctrl.loading.cards = true;
            $http['delete']('./api/cards/' + card.id).then(function (result) {
                ctrl.cardsList = result.data;
                ctrl.loading.cards = false;
            }, function (err) {
                console.log(err);
                ctrl.loading.cards = false;
            });
        }

        ctrl.log = function (data) {
            console.log(data);
        }

        ctrl.getPaymentsList();
        ctrl.getCardsList();


        ctrl.newPaypalPayment = angular.copy(defaultNewPayment);

        ctrl.newCard = angular.copy(defaultNewCard);


        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == variable) { return pair[1]; }
            }
            return (false);
        }


        function executePayment(data) {
            ctrl.loading.payments = true;
            $http.post('./api/payments/execute', data).then(function (result) {
                ctrl.paymentsList = result.data;
                ctrl.loading.payments = false;
            }, function (err) {
                ctrl.loading.payments = false;
                console.log(err);
            });
        }

        var paymentId = getQueryVariable('paymentId');
        var PayerID = getQueryVariable('PayerID');

        if (paymentId && PayerID) {
            executePayment({
                paymentId: paymentId,
                PayerID: PayerID
            });
        }

    }

    app.controller('appCtrl', ['$http', AppCtrl]);


})()