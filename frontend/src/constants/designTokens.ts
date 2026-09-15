/**
 * Design Tokens - Visual Language
 * Core colors, typography, spacing, and interaction system
 */

// ============================================================================
// COLORS
// ============================================================================

export const colors = {
  // Primary - Teal/Cyan (confident, modern, not generic blue)
  primary: {
    50: '#F0FDFA',
    100: '#CCFBF1',
    200: '#99F6E4',
    300: '#5EEAD4',
    400: '#2DD4BF',
    500: '#14B8A6', // Primary action color
    600: '#0D9488',
    700: '#0F766E',
    800: '#134E4A',
    900: '#0F2F2A',
  },

  // Success - Fresh green
  success: {
    50: '#F0FDF4',
    100: '#DCFCE7',
    200: '#BBEF63',
    300: '#86EFAC',
    500: '#22C55E',
    600: '#16A34A',
    700: '#15803D',
  },

  // Warning - Amber
  warning: {
    50: '#FFFBEB',
    100: '#FEF3C7',
    200: '#FCD34D',
    400: '#FBBF24',
    500: '#F59E0B',
  },

  // Error - Soft red
  error: {
    50: '#FEF2F2',
    100: '#FEE2E2',
    200: '#FECACA',
    300: '#FCA5A5',
    500: '#EF4444',
    600: '#DC2626',
    700: '#B91C1C',
  },

  // Neutral - Refined grays
  neutral: {
    50: '#FAFAFA',
    100: '#F5F5F5',
    150: '#F0F0F0',
    200: '#E5E5E5',
    300: '#D4D4D4',
    400: '#A3A3A3',
    500: '#737373',
    600: '#525252',
    700: '#404040',
    800: '#262626',
    900: '#171717',
  },

  // Utility
  white: '#FFFFFF',
  black: '#000000',
};

// ============================================================================
// TYPOGRAPHY
// ============================================================================

export const typography = {
  // Font families
  family: {
    base: 'System',
    mono: 'Menlo',
  },

  // Font sizes (in pixels)
  fontSize: {
    xs: 12,
    sm: 14,
    base: 16,
    lg: 18,
    xl: 20,
    '2xl': 24,
    '3xl': 28,
    '4xl': 32,
  },

  // Font weights
  weight: {
    regular: '400',
    medium: '500',
    semibold: '600',
    bold: '700',
  },

  // Line heights (unitless multipliers)
  lineHeight: {
    tight: 1.2,
    normal: 1.5,
    relaxed: 1.625,
    loose: 2,
  },
};

// ============================================================================
// SPACING
// ============================================================================

export const spacing = {
  0: 0,
  1: 4,
  2: 8,
  3: 12,
  4: 16,
  5: 20,
  6: 24,
  7: 28,
  8: 32,
  10: 40,
  12: 48,
};

// ============================================================================
// BORDER RADIUS
// ============================================================================

export const borderRadius = {
  none: 0,
  sm: 4,
  base: 8,
  md: 12,
  lg: 16,
  full: 9999,
};

// ============================================================================
// SHADOWS & ELEVATION
// ============================================================================

export const shadows = {
  none: {
    shadowColor: 'transparent',
    shadowOffset: { width: 0, height: 0 },
    shadowOpacity: 0,
    shadowRadius: 0,
    elevation: 0,
  },
  // Subtle elevation for slight separation
  sm: {
    shadowColor: colors.black,
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.04,
    shadowRadius: 2,
    elevation: 2,
  },
  // Standard card elevation
  base: {
    shadowColor: colors.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.06,
    shadowRadius: 4,
    elevation: 3,
  },
  // Elevated dialog/modal
  md: {
    shadowColor: colors.black,
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.1,
    shadowRadius: 8,
    elevation: 5,
  },
};

// ============================================================================
// ANIMATIONS & TRANSITIONS
// ============================================================================

export const animations = {
  // Durations in milliseconds
  duration: {
    instant: 0,
    fast: 100,
    quick: 150,
    base: 200,
    slow: 300,
    slower: 500,
  },

  // Common easing functions (Expo/RN Reanimated compatible)
  easing: {
    easeOut: 'ease-out',
    easeIn: 'ease-in',
    easeInOut: 'ease-in-out',
  },
};

// ============================================================================
// INTERACTIVE STATES
// ============================================================================

export const states = {
  // Button/pressable opacity states
  opacity: {
    active: 1,
    hover: 0.85,
    pressed: 0.75,
    disabled: 0.5,
  },

  // Scale states for micro-interactions
  scale: {
    base: 1,
    pressed: 0.96,
    hover: 1.02,
  },
};

// ============================================================================
// Z-INDEX SYSTEM
// ============================================================================

export const zIndex = {
  base: 0,
  dropdown: 100,
  sticky: 200,
  overlay: 300,
  modal: 400,
  toast: 500,
};
