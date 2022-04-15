
function init() {
    // Стоимость за километр.
    var DELIVERY_TARIFF = 20,
        // Минимальная стоимость.
        MINIMUM_COST = 500;
    // Создание карты.
    var myMap = new ymaps.Map("map", {
        // Координаты центра карты.
        // Порядок по умолчанию: «широта, долгота».
        // Чтобы не определять координаты центра карты вручную,
        // воспользуйтесь инструментом Определение координат.
        center: [46.359950, 48.056677],
        // Уровень масштабирования. Допустимые значения:
        // от 0 (весь мир) до 19.
        zoom: 13,
        controls: []

    });

    var zoomControl = new ymaps.control.ZoomControl({
        options: {
            size: 'small',
            float: 'none',
            position: {
                bottom: 480,
                right: 10
            }
        }
    });
    myMap.controls.add(zoomControl);

    var myRoute;
    myMap.controls.remove('geolocationControl'); // удаляем геолокацию
    myMap.controls.remove('searchControl'); // удаляем поиск
    myMap.controls.remove('trafficControl'); // удаляем контроль трафика
    myMap.controls.remove('typeSelector'); // удаляем тип
    myMap.controls.remove('fullscreenControl'); // удаляем кнопку перехода в полноэкранный режим
    //myMap.controls.remove('zoomControl'); // удаляем контрол зуммирования
    myMap.controls.remove('rulerControl'); // удаляем контрол правил
        


    var suggestView1 = new ymaps.SuggestView('suggest1', {      
        provider: {
            suggest: (function (request, options) {
                return ymaps.suggest("Россия, Астрахань, " + request);
            })
        },
        boundedBy: [[46.265009, 47.906103], [46.469173, 48.151967]]
    });
    var suggestView2 = new ymaps.SuggestView('suggest2', {
        provider: {
            suggest: (function (request, options) {
                return ymaps.suggest("Россия, Астрахань, " + request);
            })
        },
        boundedBy: [[46.265009, 47.906103], [46.469173, 48.151967]]
    });
   
    var placemark;

    function addRoute(startPoint, endPoint) {
        myMap.geoObjects.remove(myRoute);
        myRoute = new ymaps.multiRouter.MultiRoute({
            // Точки маршрута. Точки могут быть заданы как координатами, так и адресом. 
            referencePoints: [
                startPoint,
                endPoint
                /*'Россия, Астрахань, улица Кирова, 47',
                'Россия, Астрахань, улица Яблочкова, 44'*/
            ]
        }, {
            // Автоматически устанавливать границы карты так,
            // чтобы маршрут был виден целиком.
            boundsAutoApply: true
        });
        //var price2;
        myRoute.model.setParams({ results: 1 }, true);
        myRoute.model.events.add('requestsuccess', function () {

            var activeRoute = myRoute.getActiveRoute();
            if (activeRoute) {
                // Получим протяженность маршрута.
                var length = myRoute.getActiveRoute().properties.get("distance"),
                    // Вычислим стоимость доставки.
                    price = calculate(Math.round(length.value / 1000)),
                    // Создадим макет содержимого балуна маршрута.
                    balloonContentLayout = ymaps.templateLayoutFactory.createClass(
                        '<span>Расстояние: ' + length.text + '.</span><br/>' +
                        '<span style="font-weight: bold; font-style: italic">Стоимость доставки: ' + price + ' р.</span>');
                //document.getElementById('price').value = price;
                // Зададим этот макет для содержимого балуна.
                myRoute.options.set('routeBalloonContentLayout', balloonContentLayout);
                // Откроем балун.
                activeRoute.balloon.open();
            }
        });

        myMap.geoObjects.add(myRoute);
        //return price2;
    };
    // Функция, вычисляющая стоимость доставки.
    function calculate(routeLength) {
        return Math.max(routeLength * DELIVERY_TARIFF, MINIMUM_COST);
    }

    //from api


    function geocode(suggest) {
        // Забираем запрос из поля ввода.
        var address = null;
        var request = $(suggest).val();
        // Геокодируем введённые данные.
        ymaps.geocode(request).then(function (res) {
            var obj = res.geoObjects.get(0),
                error, hint;

            if (obj) {
                // Об оценке точности ответа геокодера можно прочитать тут: https://tech.yandex.ru/maps/doc/geocoder/desc/reference/precision-docpage/
                switch (obj.properties.get('metaDataProperty.GeocoderMetaData.precision')) {
                    case 'exact':
                        break;
                    case 'number':
                    case 'near':
                    case 'range':
                        error = 'Неточный адрес, требуется уточнение';
                        hint = 'Уточните номер дома';
                        break;
                    case 'street':
                        error = 'Неполный адрес, требуется уточнение';
                        hint = 'Уточните номер дома';
                        break;
                    case 'other':
                    default:
                        error = 'Неточный адрес, требуется уточнение';
                        hint = 'Уточните адрес';
                }
            } else {
                error = 'Адрес не найден';
                hint = 'Уточните адрес';
            }

            // Если геокодер возвращает пустой массив или неточный результат, то показываем ошибку.
            if (error) {
                showError(error, suggest);
                showMessage(hint);
                address = 'err'
            } else {
                address = showResult(obj, suggest);
            }
        }, function (e) {
            console.log(e)
        });
        if (address != null)
            return address;

    }
    function showResult(obj, suggest) {
        // Удаляем сообщение об ошибке, если найденный адрес совпадает с поисковым запросом.
        $(suggest).removeClass('input_error');
        $('#notice').css('display', 'none');

        var mapContainer = $('#map'),
            bounds = obj.properties.get('boundedBy'),
            // Рассчитываем видимую область для текущего положения пользователя.
            mapState = ymaps.util.bounds.getCenterAndZoom(
                bounds,
                [mapContainer.width(), mapContainer.height()]
            ),
            // Сохраняем полный адрес для сообщения под картой.
            address = [/*obj.getCountry(),*/ obj.getAddressLine()].join(', '),
            // Сохраняем укороченный адрес для подписи метки.
            shortAddress = [obj.getThoroughfare(), obj.getPremiseNumber(), obj.getPremise()].join(' ');
        // Убираем контролы с карты.
        mapState.controls = [];
        // Создаём карту.
        ////   createMap(mapState, shortAddress);
        myMap.mapState = mapState;
        // Выводим сообщение под картой.
        showMessage(address);
        return address;
    }

    function showError(message, suggest) {
        $('#notice').text(message);
        $(suggest).addClass('input_error');
        $('#notice').css('display', 'block');
        // Удаляем карту.
        /*if (map) {
            map.destroy();
            map = null;
        }*/
    }


    function checkValidCoordinates() {
        geocode('#suggest1');
        geocode('#suggest2');
    }

    function showMessage(message) {
        $('#messageHeader').text('Данные получены:');
        $('#message').text(message);
    }
    $('#button').bind('click', function (e) {
        var pointA = geocode('#suggest1');       
        var pointB = geocode('#suggest2');
        //$('#lenght').value = route.properties.get("distance").text;

        /*while (typeof pointA == 'undefined' || typeof pointB == 'undefined')
            sleep(10);
        print(pointA, pointB);
        if (pointB == 'err' || pointA == 'err')
            return;
        addRoute(pointA, pointB);*/

    });
    //Build route button
    $('#button2').bind('click', function (e) {
        checkValidCoordinates();
        addRoute($('#suggest1').val(), $('#suggest2').val());

        $.ajax({
            type: 'GET',
            url: 'CheckPrice?startpoint=' + $('#suggest1').val() + '&finishpoint=' + $('#suggest2').val(),
            success: function (result) {
                document.getElementById('suggest1').value = result.startPoint;
                document.getElementById('suggest2').value = result.finishPoint;
                $('#price').text(result.price);

            }

        })
        
    });

   /* $(document).ready(function () {
        
    })*/

}





// Функция ymaps.ready() будет вызвана, когда
// загрузятся все компоненты API, а также когда будет готово DOM-дерево.
ymaps.ready(init);
