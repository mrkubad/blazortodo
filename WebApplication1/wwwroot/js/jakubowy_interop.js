function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
window.FocusElement = async function (id) {
    await sleep(33);
    const input = document.getElementById(id);
    input.focus();
    input.select();
}

window.ConsoleLog = element => console.log(element);


window.FocusElementv2 = element => { element.focus(); element.select(); }

window.GetElementScrollWidth = element => element.scrollWidth;
window.GetElementClientWidth = function (element) {
                                    return element.clientWidth;
                               }
window.GetTextWidth = (element, fontSize) => {/*element.style.fontSize = fontSize;*/ return element.clientWidth; }

window.ScrollToElement =
function(element) 
{
sraczka = element;
    element.scrollIntoView();

    }

window.ResizeTextArea = elem => { elem.style.height = 'auto'; elem.style.height = elem.scrollHeight + 'px' };
window.SelectElement = elem => elem.select();
window.BlurElement = elem => elem.blur();
window.ChangeElementWidth = (elem, newWidth) => { if (elem.style !== undefined) { elem.style.width = newWidth + "ch" } };
window.DisableEventForElem = (elem, eventName) => { elem.addEventListener(eventName, e => e.preventDefault()); }
window.DisableEnterKeyEvent = elem => elem.addEventListener("keypress", e => { if (e.keyCode == "13") e.preventDefault(); });
window.DisableEventPropagation = (elem, eventName) => elem.addEventListener(eventName, e => e.stopPropagation());

window.Log = (text) => { console.log(text); };
window.stop = event => event.stopPropagation();


window.Sleep = async function (ms) { await sleep(ms); return true; }

//let DotNetObjectRefSidebar;


//window.SaveObjectRef = elementRef => DotNetObjectRefSidebar = elementRef;

// it will contains .net object ref for future method calls
let DotNetMainLayoutObjectRef;



window.InitObjectRef = ObjectRef => DotNetMainLayoutObjectRef = ObjectRef;


function CheckIfRowWasClicked(target, tableRowObject)
{
    do {
        if (target == tableRowObject)
            return true;

        target = target.parentNode;
    }
    while (target);
    return false;
}




function CheckMouseClick(e) {
    const rightsidebar = document.querySelector(".right-sidebar");
    const leftsidebar = document.querySelector(".left-sidebar");
    const gridContainer = document.querySelector(".grid-container");
    const mainTableRows = document.querySelectorAll(".task-element");
    
    let leftSidebarClicked = false;
    let rightSidebarClicked = false;
    let rowWasClicked = false;

    let target = e.target;

    if (mainTableRows != null) {
        for (let i = 0; i < mainTableRows.length; ++i) {

            rowWasClicked = CheckIfRowWasClicked(target, mainTableRows[i]);
            if (rowWasClicked)
                break;
        }
    }

    do {
        switch (target)
        {
            case leftsidebar:
                leftSidebarClicked = true;
                break;
            case rightsidebar:
                rightSidebarClicked = true;

                break;
            default:
              
        }
        if (target.parentNode === null)
        {
            break;
        }
        else
            target = target.parentNode;
    }
    while (target != gridContainer);


    if (!rightSidebarClicked && !rowWasClicked) {
        DotNetMainLayoutObjectRef.invokeMethodAsync("HideRightSidebar");
    }
    if (!leftSidebarClicked) {
        DotNetMainLayoutObjectRef.invokeMethodAsync("HideLeftSidebar");
    }

}
function ResizeTextAreas()
{
    let textareas = document.querySelectorAll("textarea.task-name");

    textareas.forEach(elem => window.ResizeTextArea(elem));
}

window.addEventListener("resize", ResizeTextAreas);
document.body.addEventListener("click", CheckMouseClick);

