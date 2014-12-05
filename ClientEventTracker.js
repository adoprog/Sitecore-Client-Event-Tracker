var eventTracker = false;

function AnalyticsPageEvent(name, text, key, data) {
    this.eventName = name;
    this.text = text;
    this.key = key;
    this.data = data;


    this.trigger = function() {
        var queryString = '';

        if (!this.eventName) {
            return;
        }

        queryString += '&' + 'eventName' + '=' + this.eventName;
        if (this.text) {
            queryString += '&' + '' + 'text' + '=' + this.text;
        }
        if (this.key) {
            queryString += '&' + '' + 'key' + '=' + this.key;
        }
        if (this.data) {
            queryString += '&' + '' + 'data' + '=' + this.data;
        }
        if (queryString != '') {
            var url = '/ClientEventTracker.ashx' + '?ra=' + eventTracker.randomstring() + queryString;
            eventTracker.request(url);
        }
    };
}

function EventTracker() {
    this.request = function(url) {
        var script = new ClientEventScript(url, true);
        script.load();
    };

    this.randomstring = function() {
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var text = "";

        for (var i = 0; i < 32; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }

        return text;
    };
}

function ClientEventScript(src, async) {
    this.src = src;
    this.async = async;

    this.load = function () {
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = this.src;
        script.async = this.async;

        var ssc = document.getElementsByTagName('script')[0];
        ssc.parentNode.insertBefore(script, ssc);
    };
}

eventTracker = new EventTracker();
