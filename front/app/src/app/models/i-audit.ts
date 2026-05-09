import { GlobalizationKey } from "@core";

export interface IAudit {
    eventId: Event; 
    by: string;
    at: string | Date;
}

export enum Entity {
    Box = 1,
    Brand = 2,
    Category = 3,
    Item = 4,
    Label = 5,
    Language = 6,
    Storage = 7,
    Transaction = 8
}

export const EntityName: Record<Entity, string> = {
    [Entity.Box]: 'Box',
    [Entity.Brand]: 'Brand',
    [Entity.Category]: 'Category',
    [Entity.Item]: 'Item',
    [Entity.Label]: 'Label',
    [Entity.Language]: 'Language',
    [Entity.Storage]: 'Storage',
    [Entity.Transaction]: 'Transaction'
};

export enum Event {
    Create = 1,
    Update = 2,
    Delete = 3,
    Read = 4,
    Move = 5,
    Reorder = 6,
    UpdateImage = 7
}

export const EventName: Record<Event, GlobalizationKey> = {
    [Event.Create]: 'Audit.EVENT_CREATE',
    [Event.Update]: 'Audit.EVENT_UPDATE',
    [Event.Delete]: 'Audit.EVENT_DELETE',
    [Event.Read]: 'Audit.EVENT_READ',
    [Event.Move]: 'Audit.EVENT_MOVE',
    [Event.Reorder]: 'Audit.EVENT_REORDER',
    [Event.UpdateImage]: 'Audit.EVENT_UPDATE_IMAGE'
};