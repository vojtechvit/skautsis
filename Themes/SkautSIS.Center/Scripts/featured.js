(function ($) {
    var activeContent = $('#featured-slideshow-active');
    var activeImage = $('#featured-slideshow-active-image');
    var activeText = $('#featured-slideshow-active-text');

    var imageTemplate = '<a href="{itemUrl}" title="{title}"><img class="img-responsive img-thumbnail" src="{imageUrl}" alt="{imageAlt}" /></a>';
    var textTemplate = '<div class="row"><div class="col-xs-12"><a href="{itemUrl}" title="{title}"><h1>{title}</h1></a><p>{body}</p></div></div><div class="row"><div class="col-xs-12"><p class="pull-right"><a class="btn" href="{itemUrl}" title="Otevřít celý článek">Čtěte dále</a></p></div></div>';

    if ($('.featured-slideshow-item').length > 1) {
        var timeoutId;
        var wasMobileScreen = true;

        $(window).resize(viewportWidthChanged);
        viewportWidthChanged();

        $(document).on("click", '.featured-slideshow-item', function () {
            setActive($(this));
            if (!isMobileScreen()) {
                window.clearTimeout(timeoutId);
                timeoutId = window.setTimeout(nextActive, 10000);
            }
        });

        activeContent.click(function () {
            if (!isMobileScreen()) {
                window.clearTimeout(timeoutId);
                timeoutId = window.setTimeout(nextActive, 10000);
            }
        });
    }

    function viewportWidthChanged() {
        if (isMobileScreen()) {
            window.clearTimeout(timeoutId);
            wasMobileScreen = true;
        }
        else {
            if (wasMobileScreen) {
                timeoutId = window.setTimeout(nextActive, 6000);
            }
            wasMobileScreen = false;
        }
    }

    function isMobileScreen() {
        return $(window).width() < 768;
    }

    function nextActive() {
        var active = $('.featured-slideshow-item.active');
        var next;
        if (active.nextAll('.featured-slideshow-item').length > 0)
            next = active.nextAll('.featured-slideshow-item').first();
        else
            next = $('.featured-slideshow-item').first();

        setActive(next);
        timeoutId = window.setTimeout(nextActive, 6000);
    }

    function setActive(item) {
        var itemUrl = item.data('featured-url');
        var imageUrl = item.data('featured-image-url');
        var imageAlt = item.data('featured-image-alt');
        var title = item.data('featured-title');
        var body = item.data('featured-body');

        var imageContentHtml = "";

        if (imageUrl)
            imageContentHtml = imageTemplate.replace(/\{itemUrl\}/g, itemUrl).replace(/\{imageUrl\}/g, imageUrl).replace(/\{imageAlt\}/g, imageAlt).replace(/\{title\}/g, title);

        var textContentHtml = textTemplate.replace(/\{itemUrl\}/g, itemUrl).replace(/\{title\}/g, title).replace(/\{body\}/g, body);

        activeContent.fadeOut('300', function () {
            activeImage.html(imageContentHtml);
            activeText.html(textContentHtml);
            activeContent.fadeIn();
        });

        $('.featured-slideshow-item').removeClass('active');
        item.addClass('active');
    }

})(jQuery);