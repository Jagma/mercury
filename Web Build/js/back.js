// Prevent back

var backCount = 0;

setInterval(resetCount, 500);
function resetCount () {
  backCount = 0;
  console.log("reset");
}

window.addEventListener('popstate', function (event)
{
  backCount ++;
  if (backCount >= 2) {
    // normal back behaviour
  } else {
    history.pushState(null, document.title, location.href);
  }  
});

history.pushState(null, document.title, location.href);

