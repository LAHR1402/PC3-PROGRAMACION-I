// Función para generar un ID único por usuario
function generateUserId() {
    return 'user_' + Math.random().toString(36).substr(2, 9);
}

// Función para obtener el ID del usuario actual
function getUserId() {
    // En producción usaríamos el ID real del usuario
    // Por ahora generamos uno único
    return sessionStorage.getItem('userId') || generateUserId();
}

// Guardar el ID del usuario en sessionStorage
function initializeUserId() {
    const userId = getUserId();
    sessionStorage.setItem('userId', userId);
    return userId;
}

// Exportar funciones
export { generateUserId, getUserId, initializeUserId };
