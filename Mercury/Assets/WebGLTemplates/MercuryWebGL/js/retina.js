    // Retina display support **************************************************
    var metaTag=document.createElement('meta');
    metaTag.name = "viewport";
    metaTag.content = "width=device-width, initial-scale=" + (1.0/window.devicePixelRatio).toString() + ", maximum-scale=1.0, user-scalable=0";
    document.getElementsByTagName('head')[0].appendChild(metaTag);