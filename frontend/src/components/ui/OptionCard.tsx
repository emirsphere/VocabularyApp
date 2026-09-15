/**
 * OptionCard Component
 * Interactive answer option with visual states and subtle depth
 */

import React, { useRef } from 'react';
import { Pressable, Text, StyleSheet, Animated, ViewStyle } from 'react-native';
import { colors, typography, spacing, borderRadius, shadows, states } from '@/constants/designTokens';

type OptionState = 'default' | 'selected' | 'correct' | 'wrong' | 'disabled';

interface OptionCardProps {
  text: string;
  isSelected?: boolean;
  isCorrect?: boolean;
  isWrong?: boolean;
  isDisabled?: boolean;
  onPress?: () => void;
  style?: ViewStyle;
  testID?: string;
}

export function OptionCard({
  text,
  isSelected = false,
  isCorrect = false,
  isWrong = false,
  isDisabled = false,
  onPress,
  style,
  testID,
}: OptionCardProps) {
  const scaleAnim = useRef(new Animated.Value(1)).current;

  const handlePressIn = () => {
    if (!isDisabled && !isCorrect && !isWrong) {
      Animated.spring(scaleAnim, {
        toValue: states.scale.pressed,
        useNativeDriver: true,
      }).start();
    }
  };

  const handlePressOut = () => {
    if (!isDisabled && !isCorrect && !isWrong) {
      Animated.spring(scaleAnim, {
        toValue: states.scale.base,
        useNativeDriver: true,
      }).start();
    }
  };

  const getState = (): OptionState => {
    if (isCorrect) return 'correct';
    if (isWrong) return 'wrong';
    if (isSelected) return 'selected';
    if (isDisabled) return 'disabled';
    return 'default';
  };

  const getColors = () => {
    const state = getState();
    switch (state) {
      case 'correct':
        return {
          bg: colors.success[50],
          border: colors.success[500],
          text: colors.neutral[900],
          icon: colors.success[600],
        };
      case 'wrong':
        return {
          bg: colors.error[50],
          border: colors.error[500],
          text: colors.neutral[900],
          icon: colors.error[600],
        };
      case 'selected':
        return {
          bg: colors.primary[50],
          border: colors.primary[500],
          text: colors.neutral[900],
          icon: colors.primary[600],
        };
      case 'disabled':
        return {
          bg: colors.neutral[50],
          border: colors.neutral[200],
          text: colors.neutral[400],
          icon: colors.neutral[300],
        };
      default:
        return {
          bg: colors.white,
          border: colors.neutral[200],
          text: colors.neutral[900],
          icon: colors.neutral[300],
        };
    }
  };

  const colorScheme = getColors();

  return (
    <Animated.View
      style={[
        {
          transform: [{ scale: scaleAnim }],
        },
      ]}
    >
      <Pressable
        onPress={onPress}
        onPressIn={handlePressIn}
        onPressOut={handlePressOut}
        disabled={isDisabled}
        style={({ pressed }) => [
          styles.container,
          {
            backgroundColor: colorScheme.bg,
            borderColor: colorScheme.border,
            borderWidth: getState() === 'default' ? 1 : 2,
            opacity: pressed && !isDisabled && !isCorrect && !isWrong ? states.opacity.pressed : 1,
          },
          shadows.sm,
          style,
        ]}
        testID={testID}
      >
        <Text
          style={[
            styles.text,
            {
              color: colorScheme.text,
            },
          ]}
        >
          {text}
        </Text>

        {/* Visual indicators for state */}
        {isCorrect && (
          <Text
            style={[
              styles.stateIcon,
              {
                color: colorScheme.icon,
              },
            ]}
          >
            ✓
          </Text>
        )}
        {isWrong && (
          <Text
            style={[
              styles.stateIcon,
              {
                color: colorScheme.icon,
              },
            ]}
          >
            ✕
          </Text>
        )}
      </Pressable>
    </Animated.View>
  );
}

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    borderRadius: borderRadius.md,
    paddingVertical: spacing[3],
    paddingHorizontal: spacing[4],
    minHeight: 56,
  },
  text: {
    fontSize: typography.fontSize.base,
    fontWeight: typography.weight.medium as any,
    lineHeight: typography.lineHeight.normal * typography.fontSize.base,
    flex: 1,
  },
  stateIcon: {
    fontSize: typography.fontSize.lg,
    fontWeight: typography.weight.bold as any,
    marginLeft: spacing[2],
  },
});
