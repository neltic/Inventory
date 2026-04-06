export const ERROR_FORM_MESSAGES: Record<string, (name: string) => string> = {
    required: (name) => `You must enter: ${name}`,
    minlength: (name) => `The ${name} is too short`,
    maxlength: (name) => `The ${name} is too long`,
    min: (_) => `The value must be greater than 0`,
    brandNotFound: (name) => `The selected ${name} is invalid`,
    categoryNotFound: (name) => `The selected ${name} is invalid`,
    entityNotFound: (name) => `The selected ${name} is invalid`,
    invalidDestination: (_) => `Please select a valid destination`,
    requiredTrue: (_) => `You must accept the change to continue.`,
};