import ApiProvider from './ApiProvider';
import LocalStorageProvider from './LocalStorageProvider';

export function createProvider(type) {
    switch (type) {
        case 'api': return new ApiProvider();
        case 'local': return new LocalStorageProvider();
        default: throw new Error('Unknown provider type');
    }
}