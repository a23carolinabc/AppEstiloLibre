let mouseUpListenerRef = null;

window.obtenerPosicionCanvas = function (canvasElement) {
    const rect = canvasElement.getBoundingClientRect();
    return {
        left: rect.left + window.scrollX,
        top: rect.top + window.scrollY,
        width: rect.width,
        height: rect.height
    };
};

window.registrarListenerGlobalMouseUp = function (dotNetRef) {
    // Limpiar listener anterior si existe
    if (mouseUpListenerRef) {
        document.removeEventListener('mouseup', mouseUpListenerRef);
    }

    // Crear nuevo listener
    mouseUpListenerRef = function () {
        dotNetRef.invokeMethodAsync('OnGlobalMouseUp');
    };

    // Registrar en todo el documento
    document.addEventListener('mouseup', mouseUpListenerRef);
};

window.limpiarListenerGlobalMouseUp = function () {
    if (mouseUpListenerRef) {
        document.removeEventListener('mouseup', mouseUpListenerRef);
        mouseUpListenerRef = null;
    }
};


//<script>
//    let mouseUpListenerRef = null;

//    window.obtenerPosicionCanvas = function (canvasElement) {
//            const rect = canvasElement.getBoundingClientRect();
//    return {
//        left: rect.left + window.scrollX,
//    top: rect.top + window.scrollY,
//    width: rect.width,
//    height: rect.height
//            };
//        };

//    window.registrarListenerGlobalMouseUp = function (dotNetRef) {
//            // Limpiar listener anterior si existe
//            if (mouseUpListenerRef) {
//        document.removeEventListener('mouseup', mouseUpListenerRef);
//            }

//    // Crear nuevo listener
//    mouseUpListenerRef = function () {
//        dotNetRef.invokeMethodAsync('OnGlobalMouseUp');
//            };

//    // Registrar en todo el documento
//    document.addEventListener('mouseup', mouseUpListenerRef);
//        };

//    window.limpiarListenerGlobalMouseUp = function () {
//            if (mouseUpListenerRef) {
//        document.removeEventListener('mouseup', mouseUpListenerRef);
//    mouseUpListenerRef = null;
//            }
//        };
//</script>