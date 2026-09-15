/**
 * Button Component
 * Reusable button with variants, states, and interaction feedback
 */

import React from 'react';
import { Pressable, Text, StyleSheet, ViewStyle } from 'react-native';
import { colors, typography, spacing, borderRadius, animations, states } from '@/constants/designTokens';

type ButtonVariant = 'primary' | 'secondary' | 'tertiary';
type ButtonSize = 'sm' | 'md' | 'lg';

interface ButtonProps {
  onPress: () => void;
  disabled?: boolean;
  loading?: boolean;
  variant?: ButtonVariant;
  size?: ButtonSize;
  children: React.ReactNode;
  style?: ViewStyle;
  testID?: string;
}

export function Button({
  onPress,
  disabled = false,
  loading = false,
  variant = 'primary',
  size = 'md',
  children,
  style,
  testID,
}: ButtonProps) {
  const isDisabled = disabled || loading;

  const getBackgroundColor = () => {
    if (isDisabled) return colors.neutral[200];
    if (variant === 'primary') return colors.primary[500];
    if (variant === 'secondary') return colors.neutral[100];
    return 'transparent';
  };

  const getTextColor = () => {
    if (isDisabled) return colors.neutral[400];
    if (variant === 'primary') return colors.white;
    if (variant === 'secondary') return colors.neutral[900];
    return colors.primary[500];
  };

  const getPaddingVertical = () => {
    if (size === 'sm') return spacing[2];
    if (size === 'lg') return spacing[4];
    return spacing[3];
  };

  const getPaddingHorizontal = () => {
    if (size === 'sm') return spacing[3];
    if (size === 'lg') return spacing[5];
    return spacing[4];
  };

  const getFontSize = () => {
    if (size === 'sm') return typography.fontSize.sm;
    if (size === 'lg') return typography.fontSize.lg;
    return typography.fontSize.base;
  };

  return (
    <Pressable
      onPress={onPress}
      disabled={isDisabled}
      style={({ pressed }) => [
        styles.button,
        {
          backgroundColor: getBackgroundColor(),
          opacity: pressed && !isDisabled ? states.opacity.pressed : states.opacity.active,
          transform: [
            {
              scale: pressed && !isDisabled ? states.scale.pressed : states.scale.base,
            },
          ],
          paddingVertical: getPaddingVertical(),
          paddingHorizontal: getPaddingHorizontal(),
        },
        variant === 'tertiary' && styles.borderless,
        variant !== 'primary' && styles.secondaryBorder,
        style,
      ]}
      testID={testID}
    >
      <Text
        style={[
          styles.text,
          {
            color: getTextColor(),
            fontSize: getFontSize(),
          },
        ]}
      >
        {loading ? 'Loading...' : children}
      </Text>
    </Pressable>
  );
}

const styles = StyleSheet.create({
  button: {
    borderRadius: borderRadius.md,
    alignItems: 'center',
    justifyContent: 'center',
  },
  text: {
    fontWeight: typography.weight.semibold as any,
    textAlign: 'center',
  },
  borderless: {
    backgroundColor: 'transparent',
  },
  secondaryBorder: {
    borderWidth: 1,
    borderColor: colors.neutral[200],
  },
});
