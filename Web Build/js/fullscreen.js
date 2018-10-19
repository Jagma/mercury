
// Fullscreen support ******************************************************
var doc = document;
var docEl = doc.documentElement;

function requestFullscreen () {
  var requestFullScreen = docEl.requestFullscreen || docEl.mozRequestFullScreen || docEl.webkitRequestFullScreen || docEl.msRequestFullscreen;
  if(!doc.fullscreenElement && !doc.mozFullScreenElement && !doc.webkitFullscreenElement && !doc.msFullscreenElement) {
    requestFullScreen.call(docEl);
  }
}

function exitFullscreen () {
  var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen;
  if(doc.fullscreenElement || doc.mozFullScreenElement || doc.webkitFullscreenElement || doc.msFullscreenElement) {
    cancelFullScreen.call(doc);
  }
}

window.onload=function(){
  addFullscreenDiv();
  
  if('onfullscreenchange' in document){
    document.addEventListener('fullscreenchange', fullScreenHandler);
  }
  if('onmozfullscreenchange' in document){
    document.addEventListener('mozfullscreenchange', fullScreenHandler);
  }
  if('onwebkitfullscreenchange' in document){
    document.addEventListener('webkitfullscreenchange', fullScreenHandler);
  }
  if('onmsfullscreenchange' in document){
    document.onmsfullscreenchange = fullScreenHandler;
  }

  function fullScreenHandler (event) {
    if (document.webkitIsFullScreen == false || 
        document.mozFullScreen == false) {
      addFullscreenDiv();
    }
  }
  
  function removeFullscreenDiv() {
    var body = document.getElementsByTagName("BODY")[0];
    body.removeChild(document.getElementById("fullScreenToggle").parentElement);
  }

  function addFullscreenDiv() {
    // create fullscreen div
    var div = document.createElement('div');
    var body = document.getElementsByTagName("BODY")[0];
    body.appendChild(div);
    div.innerHTML = '<div id="fullScreenToggle"></div>';

    // Add click event listener
    div.addEventListener("click", function() {
      requestFullscreen();
      removeFullscreenDiv();
    }, false);
  }
}