// Eliminación del fondo de las imágenes usando @imgly/background-removal
import * as eliminacionFondo from 'https://esm.sh/@imgly/background-removal@1.7.0';

window.eliminarFondo = async function (imagenDatos) {
    try {
        if (!imagenDatos) {
            throw new Error('No se proporcionó imagen');
        }

        // Convertir dataURL a Blob.
        const respuesta = await fetch(imagenDatos);
        const blob = await respuesta.blob();

        // Llamar a removeBackground.
        const resultadoBlob = await eliminacionFondo.removeBackground(blob, {
            model: 'isnet_fp16',
            debug: true,
        });

        // Convertir resultado a dataURL.
        return await new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onloadend = () => resolve(reader.result);
            reader.onerror = reject;
            reader.readAsDataURL(resultadoBlob);
        });

    } catch (err) {
        console.error('Error en la librería de eliminación de fondo:', err);
        throw err;
    }
};