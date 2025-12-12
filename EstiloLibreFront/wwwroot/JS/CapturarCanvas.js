// Captura de canvas usando html2canvas
window.capturarCanvas = async function (elementoOSelector) {
    try {
        let elemento;

        elemento = document.querySelector(elementoOSelector);

        // Si no se encuentra, esperar un poco y reintentar
        if (!elemento) {
            await new Promise(resolve => setTimeout(resolve, 100));
            elemento = document.querySelector(elementoOSelector);
        }

        // Verificar que el elemento es válido
        if (!elemento || !elemento.nodeType) {
            console.error('Elemento no encontrado o no válido:', elementoOSelector);
            throw new Error('Elemento no válido para captura. Selector: ' + elementoOSelector);
        }

        // Usar html2canvas para capturar el elemento
        const canvas = await html2canvas(elemento, {
            backgroundColor: '#ffffff',
            scale: 2,
            logging: false,
            useCORS: true
        });

        const imagenBase64 = canvas.toDataURL('image/png');

        // Extraer solo la parte base64
        const base64 = imagenBase64.split(',')[1];
        return base64;

    } catch (error) {
        console.error('Error en capturarCanvas:', error);
        throw error;
    }
};